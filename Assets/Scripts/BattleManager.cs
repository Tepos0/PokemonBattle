using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BattleManager : MonoBehaviour
{
    private List<Fighter> _fighters = new List<Fighter>();
    public void AddFighter(Fighter fighter)
    {
        if (fighter != null && !_fighters.Contains(fighter))
        {
            _fighters.Add(fighter);
        }
        else
        {
            Debug.LogWarning("Fighter is null or already added.");
        }
    }
    public void RemoveFighter(Fighter fighter)
    {
        if (fighter != null && _fighters.Contains(fighter))
        {
            _fighters.Remove(fighter);
        }
        else
        {
            Debug.LogWarning("Fighter is null or not found in the list.");
        }
    }
}
