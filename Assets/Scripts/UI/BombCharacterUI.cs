using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WorldToCanvas;

public class BombCharacterUI : W2C
{
    private BombCharacter _character;
    
    [SerializeField] 
    private TextMeshProUGUI[] _numbers;

    [SerializeField] private int smallest;
    [SerializeField] private int middle;
    [SerializeField] private int largest;
    
    public void Init(BombCharacter character)
    {
        _character = character;
        SetPosition(_character.transform);
        UpdateTime(character.RemainingTime);
    }

    public void ProcessUpdate(BombCharacter character)
    {
        UpdateTime(character.RemainingTime);
    }

    private void UpdateTime(int remaining)
    {
        _numbers[0].gameObject.SetActive(remaining > 99);

        smallest = remaining % 10;
        middle = ((remaining % 100) - smallest) / 10;
        largest = (remaining - middle - smallest) / 100;

        _numbers[0].text = largest.ToString();
        _numbers[1].text = middle.ToString();
        _numbers[2].text = smallest.ToString();
    }
}
