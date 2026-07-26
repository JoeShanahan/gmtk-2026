using UnityEngine;

public class PlayAudioRespectSettings : MonoBehaviour
{
    [SerializeField] 
    private AudioSource _sound;

    [SerializeField] 
    private GameSettings _settings;

    [SerializeField] private float _volume = 1;

    [SerializeField] private float _minRandomPitch = 0.9f;
    [SerializeField] private float _maxRandomPitch = 0.9f;

    private float _myPitch;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myPitch = Random.Range(_minRandomPitch, _maxRandomPitch);
        _sound.volume = _settings.SoundEnabled ? _volume : 0;
    }

    // Update is called once per frame
    void Update()
    {
        _sound.pitch = Time.timeScale * _myPitch;

    }
}
