using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private BombType _type;

    [SerializeField] private int _bombLife =  50;
    [SerializeField] private int _respawnTime = 100;
    [SerializeField] private int _delay;
    
    [SerializeField] private BombLookup _lookup;

    private float _currentTime;
    
    private void OnValidate()
    {
        Debug.Assert(_delay < _respawnTime, "Delay must be smaller than the respawn time!");
    }

    private void Start()
    {
        _currentTime = _delay;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime * 10;

        if (_currentTime <= 0)
        {
            _currentTime += _respawnTime;
            DoSpawn();
        }
    }

    private void DoSpawn()
    {
        if (transform.childCount > 10)
        {
            Debug.LogError("Too many bombs from one spawner!");
            return;
        }
        
        BombDefinition bdata = _lookup.GetData(_type);
        GameObject newObj = Instantiate(bdata.Prefab, transform);

        newObj.GetComponent<BombCharacter>().Init(_bombLife);
    }
}
