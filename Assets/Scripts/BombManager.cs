using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] 
    private List<BombCharacter> _allBombs;

    private BombCharacter _selectedBomb;
    
    public static void Register(BombCharacter character) => Instance?.RegisterBomb(character);
    public static void Unregister(BombCharacter character) => Instance?.UnregisterBomb(character);

    private static BombManager Instance
    {
        get
        {
            var bombMan = FindAnyObjectByType<BombManager>();

            if (bombMan == null)
            {
                Debug.LogError("No BombManager in the scene! You need one! aaaaaaaaah!");
            }

            return bombMan;
        }
    }

    private void Update()
    {
        if (_allBombs.Count == 0)
            return;

        if (_selectedBomb == null)
        {
            _allBombs = _allBombs.OrderBy(b => b.RemainingTime).ToList();
            _allBombs[0].TakeControlOf();
            _selectedBomb = _allBombs[0];
        }
    }
    
    private void RegisterBomb(BombCharacter character)
    {
        _allBombs.Add(character);
    }
    
    private void UnregisterBomb(BombCharacter character)
    {
        _allBombs.Remove(character);

        if (_selectedBomb == character)
        {
            _selectedBomb = null;
        }
    }
}
