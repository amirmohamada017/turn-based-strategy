using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    
    private List<Unit> _units;
    private List<Unit> _friendlyUnits;
    private List<Unit> _enemyUnits;

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Awake()
    {
        Instance = this;
        
        _units = new List<Unit>();
        _friendlyUnits = new List<Unit>();
        _enemyUnits = new List<Unit>();
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        var unit = sender as Unit;

        _units.Add(unit);
        if (unit.IsEnemy()) 
            _enemyUnits.Add(unit);
        else
            _friendlyUnits.Add(unit);
    }
    
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        var unit = sender as Unit;

        _units.Remove(unit);
        if (unit.IsEnemy()) 
            _enemyUnits.Remove(unit);
        else 
            _friendlyUnits.Remove(unit);
    }

    public List<Unit> GetUnits()
    {
        return _units;
    }
    
    public List<Unit> GetEnemyUnits()
    {
        return _enemyUnits;
    }
    
    public List<Unit> GetFriendlyUnits()
    {
        return _friendlyUnits;
    }
}
