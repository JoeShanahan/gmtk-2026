using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleIfSelected : MonoBehaviour
{
    public float selectedScale = 1.2f;
    public float normalScale = 1f;
    public float speed = 10f;

    private void OnEnable()
    {
        transform.localScale = new Vector3(normalScale, normalScale, normalScale);

    }

    void Update()
    {
        float target = (EventSystem.current.currentSelectedGameObject == gameObject)
            ? selectedScale
            : normalScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            Vector3.one * target,
            Time.deltaTime * speed
        );
    }
}
