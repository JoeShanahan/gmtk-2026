using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TeleportableObject : MonoBehaviour
{
    private Vector3 _airPosition = new Vector3(0, 90, 0);
    
    public void HangInAir(float time, Teleport exit)
    {
        StartCoroutine(HangRoutine(time, exit));
    }

    private IEnumerator HangRoutine(float time, Teleport exit)
    {
        float scaleTime = 0.1f;

        while (scaleTime > 0)
        {
            scaleTime -= Time.deltaTime;
            scaleTime = Mathf.Max(scaleTime, 0);
            transform.localScale = Vector3.one * (scaleTime * 10);
            yield return null;
        }
        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.Move(_airPosition, Quaternion.identity);
        
        
        while (time > 0)
        {
            transform.position = _airPosition;
            rb.linearVelocity = Vector3.zero;
            time -= Time.deltaTime;
            yield return null;
        }

        transform.DOScale(1, 0.1f);
        exit.LetMeLeave(rb);
    }
}
