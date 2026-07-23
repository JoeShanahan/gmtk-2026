using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WorldToCanvas;

public class BombCharacterUI : W2C
{
    private BombCharacter _character;
    
    [SerializeField] 
    private TextMeshProUGUI[] _numbers;

    [SerializeField] 
    private Transform[] _activeArrows;
    
    public void Init(BombCharacter character)
    {
        _character = character;
        SetPosition(_character.transform);
        UpdateTime(character.RemainingTime);

        foreach (Transform t in _activeArrows)
        {
            t.localScale = Vector3.zero;
        }
    }

    public void ProcessUpdate(BombCharacter character)
    {
        UpdateTime(character.RemainingTime);
        UpdateSelectedArrows(character.IsBeingControlled);
    }

    private void UpdateTime(int remaining)
    {
        _numbers[0].gameObject.SetActive(remaining > 99);

        int smallest = remaining % 10;
        int middle = ((remaining % 100) - smallest) / 10;
        int largest = (remaining - middle - smallest) / 100;

        _numbers[0].text = largest.ToString();
        _numbers[1].text = middle.ToString();
        _numbers[2].text = smallest.ToString();
    }

    private void UpdateSelectedArrows(bool isSelected)
    {
        Vector3 tgtScale = isSelected ? Vector3.one : Vector3.zero;
        
        foreach (Transform t in _activeArrows)
        {
            t.localScale = Vector3.Lerp(t.localScale, tgtScale, Time.unscaledDeltaTime * 8);
        }
    }
}
