using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentUI : MonoBehaviour
{
    private static PersistentUI _instance;
    
    [SerializeField] 
    private ScreenWipe _screenWipe;
    
    public static void DoTransition(Action transitionInComplete)
    {
        Instance?._screenWipe.DoTransition(transitionInComplete);
    }

    public static void DoTransition(string toScene)
    {
        Instance?._screenWipe.DoTransition(() => SceneManager.LoadScene(toScene));
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

    private static PersistentUI Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"No instance for {nameof(PersistentUI)}!");
                return null;
            }

            return _instance;
        }
    }
}
