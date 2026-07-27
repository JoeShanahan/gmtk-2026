using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundedDirectionExplosion : ExplosionBase
{
    [SerializeField]
    private int _pieSlices = 16;
    
    [Header("Range")]
    [SerializeField] private float _powerfulRange = 2.5f;
    [SerializeField] private float _weakRange = 2.5f;
    
    [Header("Force")]
    [SerializeField] private float _powerfulForce = 20;
    [SerializeField] private float _weakForce = 15;

    [Header("Lift")] 
    [SerializeField, Range(0, 0.5f), Header("How much to Lerp upwards (percentage)")] 
    private float _powerfulLift = 0.4f;
    
    [SerializeField, Range(0, 0.5f), Header("How much to Lerp upwards (percentage)")]  
    private float _weakLift = 0.15f;

    [SerializeField]
    private GameObject _particlePrefab;
    
    public override PotentialExplosionData[] GetPotentialExplosions()
    {
        if (_allSnapPoints == null || _allSnapPoints.Count == 0)
            GenerateAllSnapDirections();
        
        List<PotentialExplosionData> potentialExplosions = new List<PotentialExplosionData>();
        foreach (Collider col in Physics.OverlapSphere(transform.position, _powerfulRange + _weakRange))
        {
            if (col.attachedRigidbody == null)
                continue;
            
            if (col.attachedRigidbody == _myRB)
                continue;

            Vector3 diff = col.transform.position - transform.position;

            Vector3 bestLaunchVec = GetClosestDirection(diff);
            
            bool isWeakRange = diff.magnitude > _powerfulRange; 

            float lerpAmount = isWeakRange ? _weakLift : _powerfulLift;
            
            bestLaunchVec = Vector3.Lerp(bestLaunchVec, Vector3.up, lerpAmount).normalized;
            
            potentialExplosions.Add(new PotentialExplosionData(col.attachedRigidbody, this, isWeakRange, bestLaunchVec));
        }

        return potentialExplosions.ToArray();
    }
    
    protected override void GenerateAllSnapDirections()
    {
        _allSnapPoints = new List<Vector3>();
        float angle = 0;
        
        for (int i = 0; i < _pieSlices; i++)
        {
            _allSnapPoints.Add(new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle)));
            angle += 360f / _pieSlices;
            Debug.DrawLine(transform.position, transform.position + _allSnapPoints.Last(), Color.cyan, 10);
        }
    }
    
    public override void ShowExplosionRadius(LineRenderer weakRangeRenderer, LineRenderer strongRangeRenderer)
    {
        Debug.Log("Not implemented");
    }

    public override void Explode(Vector3 position, Vector3 facing)
    {
        base.Explode(position, facing);
        
        if (_allSnapPoints == null || _allSnapPoints.Count == 0)
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
