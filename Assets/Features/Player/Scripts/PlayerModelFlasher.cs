using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModelFlasher : MonoBehaviour
{
    [SerializeField] private PlayerCombatBridge _combatBridge;
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private Material _healFlashMaterial;
    [SerializeField] private float _duration = 0.15f;
    [SerializeField] private float _healDuration = 0.5f;
    private Coroutine _currentFlashCoroutine;
    
    private Dictionary<Renderer, Material[]> _renderers = new Dictionary<Renderer, Material[]>();
    
    public event Action OnMaterialsReset = delegate { };
    
    private void Awake()
    {
        var renderers = GetComponentsInChildren<Renderer>().ToList();
        foreach (var r in renderers)
        {
            if (r.gameObject.name == "Trail")
                continue;
            _renderers.Add(r, r.sharedMaterials);
        }
    }

    private void OnEnable()
    {
        _combatBridge.OnFinalDamageTaken.AddListener(OnDamaged);
    }

    private void OnDisable()
    {
        _combatBridge.OnFinalDamageTaken.RemoveListener(OnDamaged);
    }

    private void OnDamaged(int arg0, Vector3 arg1)
    {
        if(_currentFlashCoroutine != null)
            StopCoroutine(_currentFlashCoroutine);
        _currentFlashCoroutine = StartCoroutine(FlashCoroutine(_flashMaterial, _duration));
    }

    public void OnHeal()
    {
        if(_currentFlashCoroutine != null)
            StopCoroutine(_currentFlashCoroutine);
        _currentFlashCoroutine = StartCoroutine(FlashCoroutine(_healFlashMaterial, _healDuration));
    }

    private IEnumerator FlashCoroutine(Material material, float duration)
    {
        SetMaterials(material);
        yield return new WaitForSeconds(duration);
        RestoreMaterials();
        
        _currentFlashCoroutine = null;
    }

    public void SetMaterials(Material material)
    {
        foreach (var r in _renderers)
        {
            r.Key.sharedMaterials = new []{material};
        }
    }
    
    public void RestoreMaterials()
    {
        foreach (var r in _renderers)
        {
            r.Key.sharedMaterials = r.Value;
        }
        OnMaterialsReset.Invoke();
    }
}