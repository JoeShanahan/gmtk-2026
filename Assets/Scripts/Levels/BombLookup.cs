using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BombType
{
    Basic,
    Directional,
    Upwards,
    Big
}

[Serializable]
public class BombDefinition
{
    public string Desc;
    public BombType Type;
    public GameObject Prefab;
}

[CreateAssetMenu(fileName = "BombLookup", menuName = "Scriptable Objects/BombLookup")]
public class BombLookup : ScriptableObject
{


    [SerializeField] 
    private List<BombDefinition> _allBombs;

    public BombDefinition GetData(BombType btype)
    {
        return _allBombs.FirstOrDefault(b => b.Type == btype);
    }
}
