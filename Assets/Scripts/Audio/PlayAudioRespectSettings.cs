using UnityEngine;

public class PlayAudioRespectSettings : MonoBehaviour
{
    [SerializeField] 
    private AudioSource _sound;

    [SerializeField] 
    private GameSettings _settings;

    [SerializeField] private float _volume = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sound.volume = _settings.SoundEnabled ? _volume : 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
