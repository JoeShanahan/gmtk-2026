using UnityEngine;

public class ComeBackGarlic : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _startPos;
    private TrailRenderer _trail;
    
    void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.position = _startPos;
            transform.rotation = Quaternion.identity;
            _trail.Clear();
        }
    }
}
