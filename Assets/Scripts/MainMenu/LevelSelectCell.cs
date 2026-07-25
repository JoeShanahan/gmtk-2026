using TMPro;
using UnityEngine;

public class LevelSelectCell : MonoBehaviour
{
    [SerializeField] private Transform[] _stars;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(LevelData levelData, int levelNum)
    {
        _text.text = levelNum.ToString();

        foreach (Transform t in _stars)
        {
            t.gameObject.SetActive(false);
        }
    }
}
