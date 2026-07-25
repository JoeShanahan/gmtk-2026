using TMPro;
using UnityEngine;

public class LevelSelectCell : MonoBehaviour
{
    [SerializeField] private Transform[] _stars;
    [SerializeField] private TextMeshProUGUI _text;

    public void Init(LevelData levelData)
    {
        _text.text = (levelData.MainGameIndex + 1).ToString();

        foreach (Transform t in _stars)
        {
            t.gameObject.SetActive(false);
        }
    }
}
