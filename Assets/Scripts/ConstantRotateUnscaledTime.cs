using UnityEngine;

public class ConstantRotateUnscaledTime : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotateSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateSpeed * Time.unscaledDeltaTime);
    }
}
