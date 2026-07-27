using UnityEngine;

public class SpawnedLevel : MonoBehaviour
{
    [SerializeField] private Transform _mainCam;
    [SerializeField] private Transform _previewCam;
    
    public void SwapToMainCam()
    {
        _mainCam?.gameObject.SetActive(true);
        _previewCam?.gameObject.SetActive(true);
    }
}
