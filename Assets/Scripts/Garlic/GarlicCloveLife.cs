using System;
using UnityEngine;

public class GarlicCloveLife : MonoBehaviour
{
    [SerializeField] private float life;
    
    void Update()
    {
        if (life > 0)
        {
            life -= Time.deltaTime;
        } else if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
