using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ExplosionBase : MonoBehaviour
{
    public struct PotentialExplosionData
    {
        public Rigidbody Target { get; set; }
        public ExplosionBase Bomb { get; set; }
        public bool IsWeakRange { get; private set; }
        public Vector3 ExplosionDirection { get; set; }

        public PotentialExplosionData(Rigidbody target, ExplosionBase bomb, bool isWeakRange, Vector3 direction)
        {
            Target = target;
            Bomb = bomb;
            IsWeakRange = isWeakRange;
            ExplosionDirection = direction;
        }
    }
    
    [SerializeField]
    protected CharacterMovement _movement;
    
    protected List<Vector3> _allSnapPoints;
    protected Rigidbody _myRB;

    ScreenShake _screenShake;

    private void Awake()
    {
        _screenShake = FindAnyObjectByType<ScreenShake>();
    }
    
    private void OnEnable()
    {
        _myRB = GetComponent<Rigidbody>();
    }

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
        _screenShake.Shake(2f);
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
