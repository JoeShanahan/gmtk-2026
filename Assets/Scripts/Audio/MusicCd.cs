using UnityEngine;


public class MusicCd : MonoBehaviour
{
    [SerializeField] private AudioClip _music;

    public void Start()
    {
        CdPlayer.InsertDisc(_music);
    }
}
