using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerMaskRip))]
public class PlayerKnockdownManager : MonoBehaviour
{
    private PlayerMaskRip _maskRip;
    
    [field: Header("Config")]
    [field: SerializeField] public int MaxKnockdowns { get; private set; } = 2;
    [field: SerializeField, ReadOnly] public int CurrentKnockdowns { get; private set; }

    private void Awake()
    {
        _maskRip = GetComponent<PlayerMaskRip>();
    }

    [Button("Knockdown", ButtonSizes.Large)]
    public void Knockdown()
    {
        if (CurrentKnockdowns >= MaxKnockdowns)
        {
            _maskRip.Lose();
            _maskRip.OtherPlayerMaskRip.Win();
            CurrentKnockdowns++;
            return;
        }
        
        CurrentKnockdowns++;
        _maskRip.OnDeath();
    }
}