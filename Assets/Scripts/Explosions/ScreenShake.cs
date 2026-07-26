using Unity.Cinemachine;
using UnityEngine;

// Place on the main level camera!

public class ScreenShake : MonoBehaviour
{
    CinemachineCamera _camera;
    
    [SerializeField] float shake;
    float shakeAmount = 0.7f;
    float decreaseFactor = 1.0f;
    
    [SerializeField] GameSettings _settings;
    bool shakeEnabled;
    
    [SerializeField] bool doShake;
    
    void Awake()
    {
        _camera = gameObject.GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (doShake)
        {
            if (shake > 0)
            {
                _camera.transform.localPosition = Random.insideUnitSphere * (shakeAmount * _settings.ScreenShakeModifier);
                shake -= Time.deltaTime * decreaseFactor;

            }
            else
            {
                doShake = false;
                shake = 0.0f;
            }
        }
    }

    public void Shake(float length)
    {
        shake = length;
        doShake = true;
    }
}
