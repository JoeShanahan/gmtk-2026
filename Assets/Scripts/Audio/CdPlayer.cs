using UnityEngine;


// It's got a 
public class CdPlayer : MonoBehaviour
// player player player...
{
    private static CdPlayer _instance;

    [SerializeField] 
    private GameSettings _settings;

    [SerializeField] 
    private float _maxVolume = 0.5f;

    [SerializeField] 
    private AudioSource _source;

    private AudioClip _nextCd;
    
    private float _settingsModifier => _settings.MusicEnabled ? 1 : 0;
    
    public static void InsertDisc(AudioClip clip) => _instance?.ActuallyInsertDisc(clip);

    private void ActuallyInsertDisc(AudioClip clip)
    {
        if (_source.clip == clip)
            return;
        
        if (_source.clip == null)
        {
            _source.volume = _settingsModifier * _maxVolume;
            _source.clip = clip;
            _source.loop = true;
            
            if (_source.clip != null)
                _source.Play();
        }
        else
        {
            _nextCd = clip;
        }
    }

    private void Update()
    {
        float maxVolume = _settingsModifier * _maxVolume;
        
        if (_nextCd != null)
        {

            if (_source.volume > 0)
            {
                _source.volume -= Time.unscaledDeltaTime * _maxVolume * 2;
            }

            if (_source.volume <= 0)
            {
                _source.clip = _nextCd;
                _source.loop = true;
                _nextCd = null;

                if (_source.clip != null)
                {
                    _source.Play();
                }
            }
        }
        else if (_source.volume < maxVolume)
        {
            _source.volume += Time.unscaledDeltaTime * _maxVolume * 2;
            _source.volume = Mathf.Min(_source.volume, maxVolume);
        }
        else if (_source.volume > maxVolume)
        {
            _source.volume += Time.unscaledDeltaTime * _maxVolume * 3;
            _source.volume = Mathf.Min(_source.volume, maxVolume);
        }
    }
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
