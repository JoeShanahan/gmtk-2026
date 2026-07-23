using System;
using System.Collections.Generic;
using UnityEngine;

public class AllDirectionExplosion : ExplosionBase
{
    [Header("Range")]
    [SerializeField] private float _powerfulRange = 2;
    [SerializeField] private float _weakRange = 2;
    
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

    
    public override void Explode(Vector3 position, Vector3 facing)
    {
        GenerateAllSnapDirections();
        
        foreach (Collider col in Physics.OverlapSphere(position, _powerfulRange + _weakRange))
        {
            if (col.attachedRigidbody == null)
                continue;

            Vector3 diff = col.transform.position - position;

            Vector3 bestLaunchVec = GetClosestDirection(diff);

            float lerpAmount = diff.magnitude > _powerfulRange ? _weakLift : _powerfulLift;
            float force = diff.magnitude > _powerfulRange ? _weakForce : _powerfulForce;

            bestLaunchVec = Vector3.Lerp(bestLaunchVec, Vector3.up, lerpAmount).normalized;
            col.attachedRigidbody.AddForce(bestLaunchVec * force, ForceMode.Impulse);
        }
        
        GameObject newObj = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        Destroy(newObj, 8);
    }


    public void OnDrawGizmos()
    {
        Vector3 littleUp = new Vector3(0, -0.45f, 0);
        
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
    }
}
