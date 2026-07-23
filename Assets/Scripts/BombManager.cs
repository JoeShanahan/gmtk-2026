using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] 
    private List<BombCharacter> _allBombs;

    public static void Register(BombCharacter character) => Instance?.RegisterBomb(character);

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

    private IEnumerator Start()
    {
        while (_allBombs.Count == 0)
            yield return null;

        _allBombs = _allBombs.OrderBy(b => b.RemainingTime).ToList();
        _allBombs[0].TakeControlOf();
    }
    
    private void RegisterBomb(BombCharacter character)
    {
        _allBombs.Add(character);
    }
}
