using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MaskManager : MonoBehaviour
{
    [System.Serializable]
    public class TextureSpritePair
    {
        public Texture Texture;
        public Sprite Sprite;
    }
    [SerializeField] private List<TextureSpritePair> _maskTextureSpritePairs = new();
    [SerializeField, Required] private Image _playerOneMaskImage;
    [SerializeField, Required] private Image _playerTwoMaskImage;
    [SerializeField, Required] private Renderer _playerOneMaskRenderer;
    [SerializeField, Required] private Renderer _playerTwoMaskRenderer;
    [SerializeField, Required] private PlayerModelFlasher _playerOneModelFlasher;
    [SerializeField, Required] private PlayerModelFlasher _playerTwoModelFlasher;

    private TextureSpritePair _firstMaskTextureSpritePair;
    private TextureSpritePair _secondMaskTextureSpritePair;
    
    private void Start()
    {
        _firstMaskTextureSpritePair = _maskTextureSpritePairs.RandomElement();
        _secondMaskTextureSpritePair = _maskTextureSpritePairs.RandomElement();
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
        _playerOneMaskImage.sprite = _firstMaskTextureSpritePair.Sprite;
        _playerTwoMaskImage.sprite = _secondMaskTextureSpritePair.Sprite;
        _playerOneMaskRenderer.material.mainTexture = _firstMaskTextureSpritePair.Texture;
        _playerTwoMaskRenderer.material.mainTexture = _secondMaskTextureSpritePair.Texture;
    }
}