using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public List<BombCharacter> AllBombs => _allBombs;
    
    [SerializeField] 
    private List<BombCharacter> _allBombs;

    private BombCharacter _selectedBomb;

    [SerializeField]
    private bool _isPaused = true;

    private TimeManager _timeMan;

    public bool IsPaused => _isPaused;

    public void SetPause(bool isPaused)
    {
        _isPaused = isPaused;
    }
    
    public static void Register(BombCharacter character) => Instance?.RegisterBomb(character);
    public static void Unregister(BombCharacter character) => Instance?.UnregisterBomb(character);

    public static BombManager Instance
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
    
    private InputSystem_Actions _input;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    private void OnDestroy()
    {
        _input.Disable();
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

        if (_input.GameControl.Swap.WasPressedThisFrame())
        {
            HandleSwap();
        }
        else if (_input.GameControl.SwapBack.WasPressedThisFrame())
        {
            HandleSwap(true);
        }


        if (!_isPaused)
        {
            foreach (BombCharacter b in _allBombs)
            {
                b.DoUpdate();
            }
        }
    }

    public void HandleSwap(bool back=false)
    {
        if (_allBombs.Count < 2)
            return;
        
        _allBombs = _allBombs.OrderBy(b => b.RemainingTime).ToList();

        int currentIndex = _allBombs.IndexOf(_selectedBomb);
        int nextIndex = (currentIndex + 1) % _allBombs.Count;

        if (back)
        {
            nextIndex = currentIndex == 0 ? _allBombs.Count -1 : currentIndex - 1;
        }
        
        _selectedBomb.ReleaseControlOf();

        _selectedBomb = _allBombs[nextIndex];
        _selectedBomb.TakeControlOf();

        _timeMan ??= FindAnyObjectByType<TimeManager>(FindObjectsInactive.Include);
        _timeMan?.OnSwapPerformed();
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
