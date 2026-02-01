using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public int Team { get; private set; }
    [field: SerializeField] public UnityEvent<int> OnDamageTaken { get; private set; } = new();
    [field: SerializeField] public UnityEvent<int, Vector3> OnDamageTakenPosition { get; private set; } = new();

    public void ChangeTeam(int team)
    {
        Team = team;
    }

    [Button("Take Damage")]
    public void Damage(int damage, Vector3 position)
    {
        if (!enabled)
            return;
        
        OnDamageTaken?.Invoke(damage);
        OnDamageTakenPosition?.Invoke(damage, position);
    }
}