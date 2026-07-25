using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimeListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _checkmark;
    [SerializeField] private Image _icon;

    public void Init(int deciSeconds, float mySeconds)
    {
        int comparison = Mathf.RoundToInt(mySeconds * 10);
        bool isCompleted = comparison <= deciSeconds;

        if (mySeconds < 0.1f)
            isCompleted = false;
        
        float floatSeconds = deciSeconds / 10f;
        _timeText.text = floatSeconds.ToString("F1") + " s";

        _checkmark.gameObject.SetActive(isCompleted);
        _icon.gameObject.SetActive(!isCompleted);
    }

    public void SetMyTime(float mySeconds)
    {
        if (mySeconds < 0.1f)
        {
            _timeText.text = "-";
        }
        else
        {
            _timeText.text = mySeconds.ToString("F1") + " s";
        }
    }
}
