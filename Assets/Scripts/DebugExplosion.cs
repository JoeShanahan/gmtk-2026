using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugExplosion : MonoBehaviour
{
    [SerializeField] 
    private GameObject _explosionSphere;

    [SerializeField] 
    private LayerMask _floorLayer;

    [SerializeField] 
    private float _radius = 5;

    [SerializeField] 
    private float _force = 10;
    
    [SerializeField] 
    private float _upwards = 10;
    
    private Camera _cam;
    private InputSystem_Actions _input;
    
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

        if (_input.Player.Click.WasPressedThisFrame())
        {
                    
            var mousePos = _input.Player.MousePosition.ReadValue<Vector2>();

            Ray worldRay = _cam.ScreenPointToRay(mousePos);

            if (!Physics.Raycast(worldRay, out RaycastHit hit, 999, _floorLayer))
                return;

            SpawnExplosion(hit.point, _radius, _force, _upwards);
        }
    }

    private void SpawnExplosion(Vector3 pos, float size, float force, float upwards)
    {
        GameObject newObj = Instantiate(_explosionSphere);
        newObj.transform.position = pos;
        newObj.transform.localScale = Vector3.one * size;
        newObj.gameObject.SetActive(true);
        Destroy(newObj, 0.15f);

        foreach (Collider col in Physics.OverlapSphere(pos, size))
        {
            if (col.attachedRigidbody == null)
                continue;
            
            col.attachedRigidbody.AddExplosionForce(force, pos, size, upwards);
        }
    }
}
