// dylan's version of health, disregard. 

using UnityEngine;

public class HealthTest: MonoBehaviour
{
    public int maxHealth = 100;
    public int currHealth = 100;

    public GameObject damagePopupPrefab;

    public void TakeDamage(int amount)
    {
        currHealth -= amount;
        SpawnPopup(amount);

        if (currHealth <= 0)
        {
            Debug.Log(name + " died!!!");
        }
    }

    void SpawnPopup(int amount)
    {
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-0.4f, 0.4f),
            Random.Range(1f, 1.4f),
            0f
        );

        GameObject popup = Instantiate(
            damagePopupPrefab,
            spawnPos,
            Quaternion.identity
        );

        popup.GetComponent<DamagePopUpController>().SetText(amount.ToString()); 
        // ensures value of damage applied is displayed properly
    }
}