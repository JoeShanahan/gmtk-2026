using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosionBase : MonoBehaviour
{
    public struct PotentialExplosionData
    {
        public Rigidbody Target { get; set; }
        public ExplosionBase Bomb { get; set; }
        public Vector3 ExplosionDirection { get; set; }

        public PotentialExplosionData(Rigidbody target, ExplosionBase bomb, Vector3 direction)
        {
            Target = target;
            Bomb = bomb;
            ExplosionDirection = direction;
        }
    }
    
    [SerializeField]
    protected CharacterMovement _movement;
    
    protected List<Vector3> _allSnapPoints;
    
    /// <summary>
    /// each Explosion Type has such a different implementation of what's in range that they should implement this themselves
    /// </summary>
    /// <returns></returns>
    public abstract PotentialExplosionData[] GetPotentialExplosions();

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
