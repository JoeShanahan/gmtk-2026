using UnityEngine;

public class ShowIfHasChildren : MonoBehaviour
{
    [SerializeField] 
    private Transform _target;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 tgtScale = _target.childCount == 0 ? Vector3.zero : Vector3.one;
        if (tgtScale.magnitude == 0)
        {
            transform.localScale = tgtScale;
            return;
        }
        
        transform.localScale = Vector3.Lerp(transform.localScale, tgtScale, Time.deltaTime * 4);
    }
}
