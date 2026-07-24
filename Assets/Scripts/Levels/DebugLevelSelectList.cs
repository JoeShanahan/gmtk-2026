using UnityEngine;

public class DebugLevelSelectList : MonoBehaviour
{
    [SerializeField] 
    private GameObject _template;

    [SerializeField] 
    private RectTransform _listParent;
    
    public void OnEnable()
    {
        foreach (Transform t in _listParent)
        {
            if (t.gameObject == _template)
            {
                t.gameObject.SetActive(false);
                continue;
            }

            Destroy(t.gameObject);
        }
        
        foreach (LevelData data in Resources.LoadAll<LevelData>("Levels"))
        {
            GameObject newObj = Instantiate(_template, _listParent);
            newObj.gameObject.SetActive(true);
            newObj.GetComponent<DebugLevelSelectButton>().Init(data);
        }
    }

    public void SetActive(bool yesNo)
    {
        gameObject.SetActive(yesNo);
    }
}
