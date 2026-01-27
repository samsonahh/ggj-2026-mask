using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Bootstrap scene, serving as the Single Entry Point manager.
/// Handles the initial game state to a target scene inside the editor through the BootstrapConfigSO.
/// </summary>
public class Bootstrap : MonoBehaviour
{
    private static Bootstrap _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoSpawn()
    {
        if (_instance != null) return;

        var prefab = Resources.Load<GameObject>("Bootstrap");
        Instantiate(prefab);
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
