using UnityEngine;
using System;
using System.Collections;

public class ScreenWipe : MonoBehaviour
{
    [SerializeField] private RectTransform _referenceRect;
    [SerializeField] private RectTransform _blockerRect;
    [SerializeField] private RectTransform _stableRect;
    [SerializeField] private float _transitionTime = 0.2f;
    [SerializeField] private float _pauseTime = 0.1f;
    
    private bool _isInTransition;
    
    // Update is called once per frame
    void SetPosition(float t)
    {
        float screenWidth = _referenceRect.rect.width;

        float left = 0;
        float right = 0;

        if (t < 1)
        {
            right = (screenWidth + 150) * (1 - t) - 30;
            left = -30;
        }
        else
        {
            left = (screenWidth + 150) * (t - 1) - 30;
            right = -30;
        }
        
        _blockerRect.offsetMin = new Vector2(left, 0);
        _blockerRect.offsetMax = new Vector2(-right, 0);
        
        _stableRect.offsetMin = new Vector2(-left, 0);
        _stableRect.offsetMax = new Vector2(right, 0);
    }

    public void DoTransition(Action transitionInComplete)
    {
        if (_isInTransition)
            return;
        
        _isInTransition = true;
        gameObject.SetActive(true);
        StartCoroutine(TransitionRoutine(transitionInComplete));
    }

    private IEnumerator TransitionRoutine(Action transitionInComplete)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / _transitionTime;
            t = Mathf.Clamp(t, 0, 1);
            SetPosition(t);
            yield return null;
        }

        yield return new WaitForSeconds(_pauseTime - 0.05f);
        transitionInComplete?.Invoke();
        yield return new WaitForSeconds(0.05f);
        
        while (t < 2)
        {
            t += Time.deltaTime / _transitionTime;
            t = Mathf.Clamp(t, 1, 2);
            SetPosition(t);
            yield return null;
        }

        _isInTransition = false;
        gameObject.SetActive(false);
    }
}
