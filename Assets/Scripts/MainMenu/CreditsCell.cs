using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsCell : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;
    private CreditsData.Entry _person;
    
    public void Init(CreditsData.Entry person)
    {
        _text.text = person.Name;
        _icon.sprite = person.Icon;
        _person = person;
    }
}
