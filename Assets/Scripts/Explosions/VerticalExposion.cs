using System;
using System.Collections.Generic;
using UnityEngine;

public class VerticalExplosion : ExplosionBase
{
    [Header("Range")] 
    [SerializeField] private float _downwardsRange = 0.5f;
    [SerializeField] private float _powerfulRange = 2;
    [SerializeField] private float _weakRange = 2;
    [SerializeField] private float _radius = 0.8f;
    
    [Header("Force")]
    [SerializeField] private float _powerfulForce = 30;
    [SerializeField] private float _weakForce = 15;

    [SerializeField]
    private GameObject _particlePrefab;

    public override void Explode(Vector3 position, Vector3 facing)
    {
        float totalRange = _downwardsRange + _powerfulRange + _weakRange;
        Vector3 bottom = transform.position - new Vector3(0, _downwardsRange, 0);
        Vector3 top = transform.position + (Vector3.up * (_powerfulRange + _weakRange));
        Vector3 mid = Vector3.Lerp(top, bottom, 0.5f);

        Vector2 my2Dpos = new Vector2(transform.position.x, transform.position.z);
        
        foreach (Collider col in Physics.OverlapSphere(mid, totalRange / 2))
        {
            if (col.attachedRigidbody == null)
                continue;

            Vector2 their2Dpos = new Vector2(col.transform.position.x, col.transform.position.z);

            if (Vector2.Distance(my2Dpos, their2Dpos) > _radius)
                continue;

            float verticalDistance = col.transform.position.y - transform.position.y;
            Debug.Log(verticalDistance);

            bool isLargeForce = verticalDistance < _powerfulRange;
            Debug.Log(isLargeForce);

            float force = isLargeForce ? _powerfulForce : _weakForce;
            col.attachedRigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
        
        GameObject newObj = Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        Destroy(newObj, 8);
    }

    public void OnDrawGizmos()
    {
        Vector3 start = transform.position - new Vector3(0, _downwardsRange, 0);
        Vector3 powerfulEnd = transform.position + (Vector3.up *  _powerfulRange);
        Vector3 weakEnd = powerfulEnd + (Vector3.up * _weakRange);
        
        (Vector3, Color)[] todo = new[]
        {
            (start, Color.red),
            (Vector3.Lerp(start, powerfulEnd, 0.33f), Color.red),
            (Vector3.Lerp(start, powerfulEnd, 0.66f), Color.red),
            (powerfulEnd, Color.red),
            (Vector3.Lerp(powerfulEnd, weakEnd, 0.33f), Color.orange),
            (Vector3.Lerp(powerfulEnd, weakEnd, 0.66f), Color.orange),
            (weakEnd, Color.orange)
        };

        foreach ((Vector3 mid, Color col) in todo)
        {
            for (int i = 0; i < 32; i++)
            {
                float circA = (Mathf.PI * 2 / 32) * i;
                float circB = (Mathf.PI * 2 / 32) * (i + 1);

                Vector3 pointA = new Vector3(Mathf.Cos(circA) * _radius, 0, Mathf.Sin(circA) * _radius);
                Vector3 pointB = new Vector3(Mathf.Cos(circB) * _radius, 0, Mathf.Sin(circB) * _radius);
                
                Debug.DrawLine(pointA + mid, pointB + mid, col);
            }
        }
    }
}
