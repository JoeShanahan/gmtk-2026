using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleDirectionExplosion : ExplosionBase
{
    [Header("Range")]
    [SerializeField] private float _powerfulRange = 2;
    [SerializeField] private float _weakRange = 2;
    [SerializeField] private float _coneAngle = 45;
    [SerializeField] private float _coneClip = 2;
    
    [Header("Force")]
    [SerializeField] private float _powerfulForce = 20;
    [SerializeField] private float _weakForce = 10;

    [Header("Lift")] 
    [SerializeField, Range(0, 0.5f), Header("How much to Lerp upwards (percentage)")] 
    private float _powerfulLift = 0.1f;
    
    [SerializeField, Range(0, 0.5f), Header("How much to Lerp upwards (percentage)")]  
    private float _weakLift = 0.1f;

    [SerializeField]
    private GameObject _particlePrefab;

    private void Start()
    {
        GenerateAllSnapDirections();
    }
    
    public override void Explode(Vector3 position, Vector3 facing)
    {
        Vector3 startPos = transform.position - (_snappedForward * _coneClip);
        float totalRange = _coneClip + _powerfulRange + _weakRange;

        foreach (Collider col in Physics.OverlapSphere(startPos, totalRange))
        {
            if (col.attachedRigidbody == null)
                continue;

            Vector3 diff = col.transform.position - startPos;
            _snappedForward = GetClosestDirection(transform.forward);

            float angle = Mathf.Abs(Vector3.Angle(diff.normalized, _snappedForward));
            
            if (angle > _coneAngle / 2)
                continue;

            if (diff.magnitude < _coneClip)
                continue;
            
            Vector3 bestLaunchVec = GetClosestDirection(diff);
            bool isLargeForce = diff.magnitude < _coneClip + _powerfulRange;

            float lerpAmount = isLargeForce ? _powerfulLift : _weakLift;
            float force = isLargeForce ? _powerfulForce : _weakForce;

            bestLaunchVec = Vector3.Lerp(bestLaunchVec, Vector3.up, lerpAmount).normalized;
            col.attachedRigidbody.AddForce(bestLaunchVec * force, ForceMode.Impulse);
        }
        
        
        GameObject newObj = Instantiate(_particlePrefab, transform.position, Quaternion.LookRotation(facing, Vector3.up));
        Destroy(newObj, 8);
    }

    private Vector3 _cachedDirection;
    private Vector3 _snappedForward;

    public void OnDrawGizmos()
    {
        if (transform.forward != _cachedDirection)
        {
            _cachedDirection = transform.forward;
            _snappedForward = GetClosestDirection(_cachedDirection);
        }
        
        Vector3 startPos = transform.position + new Vector3(0, -0.45f, 0) - (_snappedForward * _coneClip);
        Debug.DrawLine(transform.position, transform.position + _snappedForward, Color.green);
        
        Vector3 positive = Quaternion.Euler(0f, _coneAngle * 0.5f, 0f) * _snappedForward;
        Vector3 negative = Quaternion.Euler(0f, _coneAngle * -0.5f, 0f) * _snappedForward;

        Vector3 pointA = startPos + (positive * _coneClip);
        Vector3 pointB = startPos +  (negative * _coneClip);
        Vector3 pointC = startPos + (positive * (_powerfulRange + _coneClip));
        Vector3 pointD = startPos +  (negative * (_powerfulRange + _coneClip));
        Vector3 pointE = startPos + _snappedForward * _coneClip;
        Vector3 pointF = startPos + _snappedForward * (_powerfulRange + _coneClip);

        Vector3 pointFarA = startPos + (positive * (_coneClip + _powerfulRange + _weakRange));
        Vector3 pointFarB = startPos + (negative * (_coneClip + _powerfulRange + _weakRange));
        Vector3 pointFarC = startPos + (_cachedDirection * (_coneClip + _powerfulRange + _weakRange));
        
        Debug.DrawLine(pointA, pointC, Color.red);
        Debug.DrawLine(pointB, pointD, Color.red);
        
        // Near red border
        Debug.DrawLine(pointA, pointE, Color.red);
        Debug.DrawLine(pointE, pointB, Color.red);
        
        // Far red border
        Debug.DrawLine(pointC, pointF, Color.red);
        Debug.DrawLine(pointF, pointD, Color.red);
        
        Debug.DrawLine(pointC, pointFarA, Color.orange);
        Debug.DrawLine(pointD, pointFarB, Color.orange);
        Debug.DrawLine(pointFarC, pointFarB, Color.orange);
        Debug.DrawLine(pointFarC, pointFarA, Color.orange);
    }
}
