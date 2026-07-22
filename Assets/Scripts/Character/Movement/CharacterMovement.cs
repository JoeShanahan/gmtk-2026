using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementInfo;
using System;

public class CharacterMovement : MonoBehaviour
{
    public event Action OnLanded;

    public bool IsGrounded => _ground.groundContactCount > 0;

    public Vector3 DesiredVelocity => _move.desiredVelocity;

    public Vector3 VelocityDirection => _rb.linearVelocity.magnitude > 0.01f ? _rb.linearVelocity.normalized : transform.forward;

    public Vector3 ActualVelocity => _rb.linearVelocity;

    
    [SerializeField] GroundInfo _ground;
    [SerializeField] MoveInfo _move;

    [Space(8)]
    [SerializeField] Collider _collider;
    
    Rigidbody _rb;
    bool _isLocked;
    bool _wasGroundedLastFrame = true;


    public void Launch(float force)
    {
        _isLocked = true;
        Vector3 direction = (Vector3.up + Vector3.up - transform.forward).normalized;

        _rb.linearVelocity = direction * force;
    }
    
    public void MoveForward(float amount)
    {
        Vector3 newPos = transform.position + (transform.forward * amount);
        _rb.Move(newPos, transform.rotation);
    }

    public void Start()
    {

    }

    public void SetLocked(bool isLocked)
    {
        _isLocked = isLocked;
        _move.desiredVelocity = Vector3.zero;
    }

    public void ForceSetVelocity(Vector3 vel)
    {
        _rb.linearVelocity = vel;
    }

    public void ApplyForce(Vector3 force)
    {
        _rb.AddForce(force);
    }

    public void SetDesiredDirection(Vector3 direction)
    {
        if (_isLocked)
            return;

        _move.desiredVelocity = direction * _move.tempMaxSpeed;

        direction.y = 0;
    }

    public void SetVelocity(Vector3 velocity, bool isDirection)
    {
        _move.desiredVelocity = velocity;
        _rb.linearVelocity = velocity;

        Vector3 direction = velocity.normalized;
        direction.y = 0;
    
        if (isDirection && direction.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void OnValidate () 
    {
        _ground.minGroundDotProduct = Mathf.Cos(_ground.maxSlopeAngle * Mathf.Deg2Rad);
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        OnValidate();
    }

    void ClearState () 
    {
        _ground.groundContactCount = 0;
        _ground.contactNormal = Vector3.zero;
        _ground.wallContactCount = 0;
        _ground.wallNormal = Vector3.zero;
    }

    void HandleFacingDirection()
    {
        if (_isLocked)
            return;

        if (IsGrounded && _move.velocity.magnitude > 0.1f)
        {
            Vector3 desiredDirection = _move.velocity;
            desiredDirection.y = 0;
            desiredDirection.Normalize();

            if (desiredDirection.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            }
        }
        else if (_move.desiredVelocity.magnitude > 0.1f)
        {
            Vector3 desiredDirection = _move.desiredVelocity;
            desiredDirection.y = 0;
            desiredDirection.Normalize();

            if (desiredDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 19);
            }
        }
        
    }

    void FixedUpdate()
    {
        UpdateState();
        AdjustVelocity();
        HandleFacingDirection();

        if (_rb.isKinematic == false)
            _rb.linearVelocity = _move.velocity;
    
        ClearState();
    }

    public void LogVelocity()
    {
        Vector3 o = transform.position;
        Debug.DrawLine(o, o + _rb.linearVelocity, Color.red, 10);
    }
    
    Vector3 ProjectOnContactPlane (Vector3 vector) 
    {
        return vector - _ground.contactNormal * Vector3.Dot(vector, _ground.contactNormal);
    }

    void AdjustVelocity () 
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right);
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward);

        float currentX = Vector3.Dot(_move.velocity, xAxis);
        float currentZ = Vector3.Dot(_move.velocity, zAxis);

        float acceleration = IsGrounded ? _move.maxAcceleration : _move.maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        Vector2 currentVel = new Vector2(currentX, currentZ);
        Vector2 desiredVel = new Vector2(_move.desiredVelocity.x, _move.desiredVelocity.z);

        if (_isLocked)
            desiredVel = Vector2.zero;

        // Applying the movement
        Vector2 newVel = Vector2.MoveTowards(currentVel, desiredVel, maxSpeedChange);
        _move.velocity += xAxis * (newVel.x - currentX) + zAxis * (newVel.y - currentZ);
    }

    void UpdateState () 
    {
        _ground.stepsSinceLastGrounded += 1;
        _move.velocity = _rb.linearVelocity;

        if (IsGrounded || SnapToGround() || CheckSteepContacts()) 
        {
            _ground.stepsSinceLastGrounded = 0;

            _move.tempMaxSpeed = Mathf.Max(_rb.linearVelocity.magnitude, _move.maxSpeed);
 
            if (_ground.groundContactCount > 1) {
                _ground.contactNormal.Normalize();
            }

            if (_wasGroundedLastFrame == false)
            {
                _wasGroundedLastFrame = true;
                Landed();
            }
        }
        else 
        {
            _wasGroundedLastFrame = false;
            _ground.contactNormal = Vector3.up;
        }
    }

    private void Landed()
    {
        OnLanded?.Invoke();
    }

    bool SnapToGround () 
    {
        if (_ground.stepsSinceLastGrounded > 1) 
        {
            return false;
        }
        
        float speed = _move.velocity.magnitude;
        
        if (speed > _ground.maxSnapSpeed) 
        {
            return false;
        }

        if (!Physics.Raycast(_rb.position, Vector3.down, out RaycastHit hit, _ground.probeDistance)) 
        {
            return false;
        }

        if (hit.normal.y < _ground.minGroundDotProduct) 
        {
            return false;
        }

        _ground.contactNormal = hit.normal;
        float dot = Vector3.Dot(_move.velocity, hit.normal);
        if (dot > 0f) 
        {
            _move.velocity = (_move.velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    void OnCollisionStay (Collision collision) => EvaluateCollision(collision);

    void EvaluateCollision (Collision collision) 
    {
        for (int i = 0; i < collision.contactCount; i++) 
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (normal.y >= _ground.minGroundDotProduct) 
            {
                _ground.groundContactCount += 1;
                _ground.contactNormal += normal;
            }
            else if (normal.y > -0.01f && normal.y < 0.05f) 
            {
                _ground.wallContactCount += 1;
                _ground.wallNormal += normal;
            }
        }
    }

    bool CheckSteepContacts () 
    {
        if (_ground.wallContactCount > 1) {
            _ground.wallNormal.Normalize();
            if (_ground.wallNormal.y >=_ground.minGroundDotProduct)
            {
                _ground.groundContactCount = 1;
                _ground.contactNormal = _ground.wallNormal;
                return true;
            }
        }
        return false;
    }
}