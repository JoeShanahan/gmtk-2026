using System;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] 
    private Teleport _exit;

    [SerializeField]
    private BoxCollider _collider;

    [SerializeField]
    private Vector3 _exitVelocity;

    [SerializeField] 
    private float _travelTime = 1f;

    public void LetMeLeave(Rigidbody rb)
    {
        rb.transform.position = transform.position;
        rb.Move(transform.position, Quaternion.identity);
        rb.linearVelocity = _exitVelocity;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (_exit == null)
            return;
        
        if (other.attachedRigidbody == null)
            return;

        if (other.TryGetComponent(out TeleportableObject tpo))
        {
            tpo.HangInAir(_travelTime, _exit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.2f);

        if (_collider != null)
        {
            Gizmos.DrawWireCube(transform.position + _collider.center, _collider.size);
        }

        if (_exit != null)
        {
            Gizmos.DrawLine(transform.position, _exit.transform.position);
        }

        if (_exitVelocity.magnitude > 0.01f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (_exitVelocity.normalized * 2));
        }
    }
}
