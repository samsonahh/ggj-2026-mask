using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MaskManager : MonoBehaviour
{
    [System.Serializable]
    public class MaterialSpritePair
    {
        public Material Material;
        public Sprite Sprite;
    }
    [SerializeField] private List<MaterialSpritePair> _maskTextureSpritePairs = new();
    [SerializeField, Required] private Image _playerOneMaskImage;
    [SerializeField, Required] private Image _playerTwoMaskImage;
    [SerializeField, Required] private Renderer _playerOneMaskRenderer;
    [SerializeField, Required] private Renderer _playerTwoMaskRenderer;
    [SerializeField, Required] private PlayerModelFlasher _playerOneModelFlasher;
    [SerializeField, Required] private PlayerModelFlasher _playerTwoModelFlasher;

    [SerializeField, ReadOnly] private List<MaterialSpritePair> _remainingPairs = new List<MaterialSpritePair>();
    private MaterialSpritePair _firstMaskMaterialSpritePair;
    private MaterialSpritePair _secondMaskMaterialSpritePair;
    
    private void Start()
    {
        _remainingPairs = new List<MaterialSpritePair>(_maskTextureSpritePairs);
        _firstMaskMaterialSpritePair = _remainingPairs.RandomElement();
        _remainingPairs.Remove(_firstMaskMaterialSpritePair);
        _secondMaskMaterialSpritePair = _remainingPairs.RandomElement();
        
        ApplyMasks();
        
        _playerOneModelFlasher.OnMaterialsReset += ApplyMasks;
        _playerTwoModelFlasher.OnMaterialsReset += ApplyMasks;
    }

    private void OnDestroy()
    {
        _playerOneModelFlasher.OnMaterialsReset -= ApplyMasks;
        _playerTwoModelFlasher.OnMaterialsReset -= ApplyMasks;
    }

    public void ApplyMasks()
    {
        _playerOneMaskImage.sprite = _firstMaskMaterialSpritePair.Sprite;
        _playerTwoMaskImage.sprite = _secondMaskMaterialSpritePair.Sprite;
        _playerOneMaskRenderer.sharedMaterial = _firstMaskMaterialSpritePair.Material;
        _playerTwoMaskRenderer.sharedMaterial = _secondMaskMaterialSpritePair.Material;
    }
}