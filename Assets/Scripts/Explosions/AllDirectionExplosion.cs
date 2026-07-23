using System;
using UnityEngine;

public class AllDirectionExplosion : ExplosionBase
{
    public float _powerfulRange = 2;
    public float _weakRange = 2;
    
    public void OnDrawGizmos()
    {
        Vector3 littleUp = new Vector3(0, 0.1f, 0);
        
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
