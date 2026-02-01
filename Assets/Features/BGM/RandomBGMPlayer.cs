using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBGMPlayer : MonoBehaviour
{
    [SerializeField] private List<StringAsset> _randomBgms = new List<StringAsset>();

    private void Start()
    {
        int randomIndex = Random.Range(0, _randomBgms.Count);
        AudioManager.Instance.PlayMusic(_randomBgms[randomIndex]);
    }
}
