using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GymManager : MonoBehaviour
{
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private Image[] _images;

    [SerializeField] private GameObject _basicPrefab;
    [SerializeField] private GameObject _bigPrefab;
    [SerializeField] private GameObject _claymorePrefab;
    [SerializeField] private GameObject _barrelPrefab;

    private Camera _cam;
    private InputSystem_Actions _input;

    private GameObject _currentSelection;
    
    private void HideAllHighlights()
    {
        foreach (Image img in _images)
        {
            img.gameObject.SetActive(false);
        }
    }
    
    public void SelectBasic()
    {
        HideAllHighlights();
        
        if (_currentSelection == _basicPrefab)
        {
            _currentSelection = null;
        }
        else
        {
            _images[0].gameObject.SetActive(true);
            _currentSelection = _basicPrefab;
        }
    }
    
    public void SelectBig()
    {
        HideAllHighlights();
        
        if (_currentSelection == _bigPrefab)
        {
            _currentSelection = null;
        }
        else
        {
            _images[1].gameObject.SetActive(true);
            _currentSelection = _bigPrefab;
        }
    }
    
    public void SelectClaymore()
    {
        HideAllHighlights();
        
        if (_currentSelection == _claymorePrefab)
        {
            _currentSelection = null;
        }
        else
        {
            _images[2].gameObject.SetActive(true);
            _currentSelection = _claymorePrefab;
        }
    }
    
    public void SelectBarrel()
    {
        HideAllHighlights();
        
        if (_currentSelection == _barrelPrefab)
        {
            _currentSelection = null;
        }
        else
        {
            _images[3].gameObject.SetActive(true);
            _currentSelection = _barrelPrefab;
        }
    }

    void Start()
    {
        _cam = Camera.main;
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_input.Player.Click.WasPressedThisFrame() && _currentSelection != null)
        {
                    
            var mousePos = _input.Player.MousePosition.ReadValue<Vector2>();

            Ray worldRay = _cam.ScreenPointToRay(mousePos);

            if (!Physics.Raycast(worldRay, out RaycastHit hit, 999, _floorLayer))
                return;

            Instantiate(_currentSelection, hit.point + Vector3.up, Quaternion.identity);
        }
    }
}
