using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModelFlasher : MonoBehaviour
{
    [SerializeField] private PlayerCombatBridge _combatBridge;
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private float _duration = 0.15f;
    private Coroutine _currentFlashCoroutine;
    
    private Dictionary<Renderer, Material[]> _renderers = new Dictionary<Renderer, Material[]>();
    
    private void Awake()
    {
        var renderers = GetComponentsInChildren<Renderer>().ToList();
        foreach (var r in renderers)
            _renderers.Add(r, r.sharedMaterials);
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
        _currentFlashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        SetMaterials(_flashMaterial);
        yield return new WaitForSeconds(_duration);
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
    }
}