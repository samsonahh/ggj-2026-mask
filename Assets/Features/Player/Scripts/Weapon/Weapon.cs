using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private List<CapsuleCollider> _capsuleColliders = new List<CapsuleCollider>();
    
    [SerializeField] private LayerMask _hitboxLayerMask;
    public Damageable HolderDamageable { get; private set; }
    
    public event Action<Damageable, Vector3> OnWeaponHit = delegate { };
    
    #region Between-Frame Collisions
    private bool _isCheckingCollisions = false;
    private bool _willDoDamage = true;
    private List<Transform> _colliderStartTransforms = new List<Transform>();
    private List<Transform> _colliderEndTransforms = new List<Transform>();
    private Ray[] _currentFrameCollisionRays;
    private Ray[] _previousFrameCollisionRays;
    private int _currentHitFrame;
    #endregion
    
    private float _impactFramesTimeScale;
    private float _impactFramesDuration;
    private HashSet<GameObject> _objectsHitByCurrentAttack = new HashSet<GameObject>();

    private int _damage;

    private void Awake()
    {
        _capsuleColliders = GetComponents<CapsuleCollider>().ToList();
    }

    private void Start()
    {
        PopulateColliderStartEndPositions();
        DisableTriggers();
        
        HolderDamageable = GetComponentInParent<Damageable>();
    }

    /// <summary>
    /// Creates and assigns the start and end positions for each collider attached to the weapon.
    /// This method is responsible for populating the colliderStartTransforms and colliderEndTransforms lists,
    /// as well as initializing the currentFrameCollisionRays and previousFrameCollisionRays arrays.
    /// </summary>
    private void PopulateColliderStartEndPositions()
    {
        for (int i = 0; i < _capsuleColliders.Count; i++)
        {
            GameObject start = new GameObject($"Collider{i} Start");
            GameObject end = new GameObject($"Collider{i} End");

            start.transform.SetParent(transform);
            end.transform.SetParent(transform);

            Vector3 capsuleColliderDirection = Vector3.up; // Assume the capsule collider direction is set to Y-Axis by default
            switch (_capsuleColliders[i].direction)
            {
                case 0: capsuleColliderDirection = Vector3.right; break; // X-Axis
                case 1: capsuleColliderDirection = Vector3.up; break; // Y-Axis
                case 2: capsuleColliderDirection = Vector3.forward; break; // Z-Axis
            }

            start.transform.localPosition = _capsuleColliders[i].center - (0.5f * _capsuleColliders[i].height - _capsuleColliders[i].radius) * capsuleColliderDirection;
            end.transform.localPosition = _capsuleColliders[i].center + (0.5f * _capsuleColliders[i].height - _capsuleColliders[i].radius) * capsuleColliderDirection;

            _colliderStartTransforms.Add(start.transform);
            _colliderEndTransforms.Add(end.transform);
        }

        _currentFrameCollisionRays = new Ray[_capsuleColliders.Count];
        _previousFrameCollisionRays = new Ray[_capsuleColliders.Count];
    }

    private void Update()
    {
        HandleHitDetectionBetweenFrames();
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (!_isCheckingCollisions) 
            return;
        
        if (!_willDoDamage) 
            return;
        
        if ((_hitboxLayerMask & (1 << other.gameObject.layer)) == 0) 
            return; // if not in the layer mask

        CheckHitboxCollisionsWithCollisions(other);
    }
    
    /// <summary>
    /// Handles hitbox hit detection with the colliders
    /// </summary>
    /// <param name="other">The collider hit by the trigger</param>
    private void CheckHitboxCollisionsWithCollisions(Collision other)
    {
        Vector3 hitPoint = other.collider.ClosestPointOnBounds(transform.position);
        AttemptToHit(other.collider, hitPoint, true);
    }
    
    /// <summary>
    /// Handles hit detection between frames.
    /// </summary>
    private void HandleHitDetectionBetweenFrames()
    {
        if (!_isCheckingCollisions)
        {
            _currentHitFrame = 0;
            return;
        }

        if (!_willDoDamage)
        {
            _currentHitFrame = 0;
            return;
        }

        // Loop through every capsule collider attached
        for (int i = 0; i < _capsuleColliders.Count; i++)
        {
            _previousFrameCollisionRays[i] = _currentFrameCollisionRays[i];

            // Calculate the current frame collision ray (from start to end)
            Vector3 dir = _colliderEndTransforms[i].position - _colliderStartTransforms[i].position;
            _currentFrameCollisionRays[i] = new Ray(_colliderStartTransforms[i].position, dir);

            if (_currentHitFrame > 0)
            {
                // Split the current fram ray into segments and sphere cast between each frame's segment
                int segments = (int)Mathf.Ceil(dir.magnitude / _capsuleColliders[i].radius);
                for (int s = 0; s <= segments; s++)
                {
                    Vector3 currPoint = _currentFrameCollisionRays[i].origin + s / (float)segments * _currentFrameCollisionRays[i].direction;
                    Vector3 prevPoint = _previousFrameCollisionRays[i].origin + s / (float)segments * _previousFrameCollisionRays[i].direction;

                    CheckHitsWithSphereCast(new Ray(prevPoint, currPoint - prevPoint), Vector3.Distance(currPoint, prevPoint), _capsuleColliders[i].radius * transform.lossyScale.x);

                    // Debugging
                    /*Debug.DrawLine(currPoint, prevPoint, Color.red, 2f);
                    CustomGizmos.InstantiateTemporarySphere(currPoint, _capsuleColliders[i].radius * transform.lossyScale.x, 1f,
                        Color.Lerp(new Color(1f, 0, 0, 0.1f), new Color(0, 0, 1f, 0.1f), (i + 1) / _capsuleColliders.Count));
                    CustomGizmos.InstantiateTemporarySphere(prevPoint, _capsuleColliders[i].radius * transform.lossyScale.x, 1f,
                        Color.Lerp(new Color(1f, 0, 0, 0.1f), new Color(0, 0, 1f, 0.1f), (i + 1) / _capsuleColliders.Count));*/
                }
            }
        }

        _currentHitFrame++;
    }

    /// <summary>
    /// Checks for hits using a sphere cast and attempts to hit the enemy.
    /// </summary>
    /// <param name="ray">The ray to cast.</param>
    /// <param name="distance">The distance of the sphere cast.</param>
    /// <param name="radius">The radius of the sphere cast.</param>
    private void CheckHitsWithSphereCast(Ray ray, float distance, float radius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, distance, _hitboxLayerMask);

        if (hits == null) 
            return;
        
        if (hits.Length == 0) 
            return;

        foreach (RaycastHit hit in hits)
        {
            Vector3 hitPoint = hit.collider.ClosestPointOnBounds(hit.point);
            if (hit.distance == 0) 
                hitPoint = hit.collider.ClosestPointOnBounds(transform.position);

            // do something
            AttemptToHit(hit.collider, hitPoint, false);
        }
    }
    
    /// <summary>
    /// Attempts to hit an enemy with the weapon.
    /// </summary>
    /// <param name="fromTrigger">Flag indicating if the hit is from a trigger.</param>
    private void AttemptToHit(Collider hit, Vector3 hitPoint, bool fromTrigger)
    {
        if (hit == null) 
            return;

        Damageable victim = hit.GetComponentInParent<Damageable>();
        if (victim == null)
            return;
        if (victim.Team == HolderDamageable.Team)
            return;
        
        if (_objectsHitByCurrentAttack.Contains(victim.gameObject))
            return;
        _objectsHitByCurrentAttack.Add(victim.gameObject);

        Hit(victim, hitPoint, fromTrigger);
    }
    
    /// <summary>
    /// Hits an entity with the weapon, triggering impact frames, camera shake, and damage calculation.
    /// </summary>
    /// <param name="fromTrigger">Flag indicating if the hit is from the trigger.</param>
    private void Hit(Damageable victim, Vector3 hitPoint, bool fromTrigger)
    {
        victim.Damage(_damage, hitPoint);
        OnWeaponHit?.Invoke(victim, hitPoint);
        
        TimeScaleManager.Instance.StartImpactFrames(_impactFramesTimeScale, _impactFramesDuration);
        
        // CustomGizmos.InstantiateTemporarySphere(hitPoint, 0.1f, 5f, fromTrigger ? Color.green : Color.magenta);
    }

    /// <summary>
    /// Sets the timescale and duration of the impact frames.
    /// </summary>
    /// <param name="newScale">The new timescale of the impact frames.</param>
    /// <param name="newDuration">The new duration of the impact frames.</param>
    public void ConfigureImpactFrames(float newScale, float newDuration)
    {
        _impactFramesTimeScale = newScale;
        _impactFramesDuration = newDuration;
    }

    /// <summary>
    /// Clears the list of objects hit by the current attack.
    /// </summary>
    public void ClearObjectHitList()
    {
        _objectsHitByCurrentAttack.Clear();
    }

    /// <summary>
    /// Enables all the colliders attached to the weapon.
    /// Sets the isCheckingCollisions flag to true.
    /// </summary>
    /// <param name="willDoDamage">Determines whether the trigger will do damage</param>
    public void EnableTriggers(bool willDoDamage = true)
    {
        _isCheckingCollisions = true;

        if (willDoDamage)
        {
            _willDoDamage = true;
            // trailParticle?.Play();
        }

        foreach (CapsuleCollider c in _capsuleColliders)
        {
            c.enabled = true;
        }
    }

    /// <summary>
    /// Disables all the colliders attached to the weapon.
    /// Sets the isCheckingCollisions flag to false.
    /// </summary>
    public void DisableTriggers()
    {
        _isCheckingCollisions = false;

        _willDoDamage = false;
        // trailParticle?.Stop();

        foreach (CapsuleCollider c in _capsuleColliders)
        {
            c.enabled = false;
        }
    }
    
    public void ConfigureDamage(int damage) => _damage = damage;
}