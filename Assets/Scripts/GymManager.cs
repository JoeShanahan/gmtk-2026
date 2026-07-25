using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GymManager : MonoBehaviour
{
    [SerializeField] private LayerMask _floorLayer;
    [SerializeField] private Image[] _images;

    [SerializeField] private BombLookup _lookup;

    private Camera _cam;
    private InputSystem_Actions _input;

    private BombDefinition _currentSelection;
    
    private void HideAllHighlights()
    {
        foreach (Image img in _images)
        {
            img.gameObject.SetActive(false);
        }
    }

    private bool DoSelection(BombType btype)
    {
        HideAllHighlights();

        BombDefinition thisDef = _lookup.GetData(btype);
        
        if (_currentSelection == thisDef)
        {
            _currentSelection = null;
            return false;
        }

        _currentSelection = thisDef;
        return true;
        
    }
    
    public void SelectBasic()
    {
        bool isSelected = DoSelection(BombType.Basic);
        _images[0].gameObject.SetActive(isSelected);
    }
    
    public void SelectBig()
    {
        bool isSelected = DoSelection(BombType.Big);
        _images[1].gameObject.SetActive(isSelected);
    }
    
    public void SelectClaymore()
    {
        bool isSelected = DoSelection(BombType.Directional);
        _images[2].gameObject.SetActive(isSelected);
    }
    
    public void SelectBarrel()
    {
        bool isSelected = DoSelection(BombType.Upwards);
        _images[3].gameObject.SetActive(isSelected);
    }

    void Start()
    {
        _cam = Camera.main;
        _input = new InputSystem_Actions();
        _input.Enable();
    }
    
    private void OnDestroy()
    {
        _input.Disable();
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

            Instantiate(_currentSelection.Prefab, hit.point + Vector3.up, Quaternion.identity);
        }
    }
}
