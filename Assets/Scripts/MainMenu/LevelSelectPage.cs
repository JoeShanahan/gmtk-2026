using UnityEngine;

public class LevelSelectPage : MonoBehaviour
{
    [SerializeField] private GameObject _templateCell;
    [SerializeField] private RectTransform _gridRect;
    [SerializeField] private MainGameLevelSet _levelSet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        foreach (Transform t in _gridRect)
        {
            if (t.gameObject == _templateCell)
            {
                t.gameObject.SetActive(false);
                continue;
            }
            
            Destroy(t.gameObject);
        }

        for (int i = 0; i < _levelSet.LevelCount; i++)
        {
            GameObject newObj = Instantiate(_templateCell, _gridRect);
            newObj.gameObject.SetActive(true);
            newObj.GetComponent<LevelSelectCell>().Init(_levelSet.GetLevel(i), _levelSet);
        }
    }

    private const string MAIN_SCENE_NAME = "RealGameScene";
    
    public void GoToGame()
    {
        PersistentUI.DoTransition(MAIN_SCENE_NAME);
    }
}
