using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosionBase : MonoBehaviour
{
    protected List<Vector3> _allSnapPoints;

    protected void GenerateAllSnapDirections()
    {
        _allSnapPoints = new List<Vector3>();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    var newVect = new Vector3(x, y, z);

                    if (newVect.magnitude != 0)
                    {
                        _allSnapPoints.Add(newVect.normalized);
                    }
                }
            }
        }
    }
    
    public virtual void Explode(Vector3 position, Vector3 facing)
    {
        
    }
    
    

    protected Vector3 GetClosestDirection(Vector3 originalDirection)
    {
        float bestDot = -1;
        Vector3 best = originalDirection;

        originalDirection = originalDirection.normalized;

        foreach (Vector3 v in _allSnapPoints)
        {
            float dot = Vector3.Dot(v, originalDirection);

            if (dot > bestDot)
            {
                bestDot = dot;
                best = v;
            }
        }

        return best;
    }
}
