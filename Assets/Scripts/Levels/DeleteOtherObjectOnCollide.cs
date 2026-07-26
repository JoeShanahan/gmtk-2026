using System;
using UnityEngine;

public class DeleteOtherObjectOnCollide : MonoBehaviour
{
    // Stick this script onto a GameObject with a child GameObject of the object/s you want this to remove. See "Test Key" prefab under prefab/tests to see example.

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Garlic")
        {
            Destroy(this.gameObject);
        }
    }
}
