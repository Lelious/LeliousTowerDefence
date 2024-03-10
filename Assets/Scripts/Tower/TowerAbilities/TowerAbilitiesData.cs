using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerAbilitiesData", menuName = "ScriptableObjects/Abilities/AbilitiesData", order = 1)]
public class TowerAbilitiesData : ScriptableObject
{
    [SerializeField] private List<TowerAbility> _abilities = new List<TowerAbility>();
}
