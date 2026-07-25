using UnityEngine;

public class AboutPage : MonoBehaviour
{
    [SerializeField] private GameObject _templateCell;
    [SerializeField] private RectTransform _gridRect;
    [SerializeField] private CreditsData _credits;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform t in _gridRect)
        {
            if (t.gameObject == _templateCell)
            {
                t.gameObject.SetActive(false);
            }
        }

        foreach (CreditsData.Entry person in _credits.People)
        {
            GameObject newObj = Instantiate(_templateCell, _gridRect);
            newObj.gameObject.SetActive(true);
            newObj.GetComponent<CreditsCell>().Init(person);
        }
    }
}
