using UnityEngine;

public class EnviromentalKillTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        if (other.TryGetComponent<BombCharacter>(out var bombCharacter))
        {
            other.GetComponent<BombCharacter>().Explode();
        }
    }
}
