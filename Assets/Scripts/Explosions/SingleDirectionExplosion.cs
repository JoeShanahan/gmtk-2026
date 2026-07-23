using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleDirectionExplosion : ExplosionBase
{
    [Header("Range")]
    [SerializeField] private float _powerfulRange = 2;
    [SerializeField] private float _weakRange = 2;
    [SerializeField] private float _powerfulWidth = 3;
    [SerializeField] private float _weakWidth = 5;
    
    [Header("Force")]
    [SerializeField] private float _powerfulForce = 100;
    [SerializeField] private float _weakForce = 50;

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

        GameObject newObj = Instantiate(_particlePrefab, transform.position, Quaternion.LookRotation(facing, Vector3.up));
        Destroy(newObj, 8);
    }

    private Vector3 _cachedDirection;
    private Vector3 _snappedForward;
    private Vector3 _snappedRight;

    public void OnDrawGizmos()
    {
        if (transform.forward != _cachedDirection)
        {
            _cachedDirection = transform.forward;
            _snappedForward = GetClosestDirection(_cachedDirection);
            _snappedRight = GetClosestDirection(transform.right);
        }
        
        Vector3 startPos = transform.position + new Vector3(0, -0.45f, 0);
        Debug.DrawLine(transform.position, transform.position + _snappedForward, Color.green);

        Vector3 pointA = startPos + (_snappedRight * _powerfulWidth * 0.5f);
        Vector3 pointB = startPos + (_snappedRight * _powerfulWidth * -0.5f);
        Vector3 pointC = pointA + (_snappedForward * _powerfulRange);
        Vector3 pointD = pointB + (_snappedForward * _powerfulRange);
        
        Debug.DrawLine(pointA, pointB, Color.red);
        Debug.DrawLine(pointA, pointC, Color.red);
        Debug.DrawLine(pointB, pointD, Color.red);
        Debug.DrawLine(pointC, pointD, Color.red);

        /*
        for (int i = 0; i < 32; i++)
        {
            float circA = (Mathf.PI * 2 / 32) * i;
            float circB = (Mathf.PI * 2 / 32) * (i + 1);

            Vector3 pointA = new Vector3(Mathf.Cos(circA) * _powerfulRange, 0, Mathf.Sin(circA) * _powerfulRange);
            Vector3 pointB = new Vector3(Mathf.Cos(circB) * _powerfulRange, 0, Mathf.Sin(circB) * _powerfulRange);

            Vector3 pointA2 = new Vector3(Mathf.Cos(circA) * (_powerfulRange + _weakRange), 0, Mathf.Sin(circA) * (_powerfulRange + _weakRange));
            Vector3 pointB2 = new Vector3(Mathf.Cos(circB) * (_powerfulRange + _weakRange), 0, Mathf.Sin(circB) * (_powerfulRange + _weakRange));

            pointA += transform.position + littleUp;
            pointB += transform.position + littleUp;

            pointA2 += transform.position + littleUp;
            pointB2 += transform.position + littleUp;

            if (i % 4 == 2)
            {
                Debug.DrawLine(transform.position + littleUp, pointA, Color.red);
                Debug.DrawLine(pointA, pointA2, Color.orange);
            }

            Debug.DrawLine(pointA, pointB, Color.red);
            Debug.DrawLine(pointA2, pointB2, Color.orange);
        }
        */
    }
}
