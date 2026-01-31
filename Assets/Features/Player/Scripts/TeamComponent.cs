using System.Collections.Generic;
using UnityEngine;

public class TeamComponent : MonoBehaviour
{
    [field: SerializeField] public int Team { get; private set; }

    public void ChangeTeam(int team)
    {
        Team = team;
    }
}