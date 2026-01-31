using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    [SerializeField] private DamagePopUpController _damageNumberPrefab;
    
    public void SpawnDamageNumber(int damage, Vector3 spawnPosition)
    {
        Vector3 spawnPos = spawnPosition + new Vector3(
            Random.Range(-0.4f, 0.4f),
            Random.Range(1f, 1.4f),
            0f
        );

        DamagePopUpController popup = Instantiate(
            _damageNumberPrefab,
            spawnPos,
            Quaternion.identity
        );

        popup.SetText(damage.ToString()); 
    }
}