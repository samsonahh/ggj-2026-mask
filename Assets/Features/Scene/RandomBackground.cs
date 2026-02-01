using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomBackground : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _backgroundSprites;

    private void Awake()
    {
        _spriteRenderer =  GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _backgroundSprites.RandomElement();
    }
}