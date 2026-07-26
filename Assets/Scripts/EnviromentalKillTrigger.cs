using UnityEngine;

public class EnviromentalKillTrigger : MonoBehaviour
{
    private GarlicManager _garlicManager;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BombCharacter>(out var bombCharacter))
        {
            other.GetComponent<BombCharacter>().Explode();
        }

        if (other.CompareTag("Garlic"))
        {
            if (_garlicManager == null)
            {
                _garlicManager = FindAnyObjectByType<GarlicManager>();
            }
            
            _garlicManager.RespawnGarlic(other.gameObject);
        }
    }
}
