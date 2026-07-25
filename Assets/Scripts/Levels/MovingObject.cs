using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Serializable]
    public class Keyframe
    {
        public Transform Location;
        public int Time;
    }
    
    [SerializeField] 
    private Keyframe[] _positions;

    [SerializeField] 
    private int _loopTime = 100;

    [SerializeField] 
    private int _timeAtStart = 5;
    
    [SerializeField] 
    private int _timeAtEnd = 5;
    
    private Rigidbody _rb;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private BombManager _bombManager;

    private float _currentTime;

    private void Start()
    {
        _bombManager = FindAnyObjectByType<BombManager>(FindObjectsInactive.Include);
        _rb = GetComponent<Rigidbody>();
        _startPos = _rb.position;
        _startRot = _rb.rotation;
        
        if (_rb == null)
        {
            Debug.LogError("Moving object needs a rigid body!");
        }
        else if (_rb.isKinematic == false)
        {
            Debug.LogError("Moving object needs a kinematic rigid body!");
        }
    }

    private void FixedUpdate()
    {
        if (_bombManager.IsPaused)
            return;

        _currentTime += Time.fixedDeltaTime * 10;

        if (_currentTime >= _loopTime)
        {
            _currentTime -= _loopTime;
        }
        
        (Vector3, Quaternion, int) lastTuple = (_startPos, _startRot, 0);

        foreach (var thisTuple in AllPositions())
        {
            if (_currentTime > thisTuple.Item3)
            {
                lastTuple = thisTuple;
                continue;
            }

            float percentThrough = Mathf.InverseLerp(lastTuple.Item3, thisTuple.Item3, _currentTime);
            Vector3 lerpedPos = Vector3.Lerp(lastTuple.Item1, thisTuple.Item1, percentThrough);
            Quaternion lerpedRot = Quaternion.Lerp(lastTuple.Item2, thisTuple.Item2, percentThrough);

            _rb.Move(lerpedPos, lerpedRot);
            break;
        }
    }


    private IEnumerable<(Vector3, Quaternion, int)> AllPositions()
    {
        if (Application.isPlaying == false)
            _startPos = transform.position;
        
        yield return (_startPos, _startRot, 0);
        
        if (_timeAtStart > 0)
            yield return (_startPos, _startRot, _timeAtStart);

        int lastTime = _loopTime - _timeAtEnd;
        
        foreach (Keyframe kf in _positions)
        {
            if (kf.Time < lastTime && kf.Location != null)
                yield return (kf.Location.position, kf.Location.rotation, kf.Time);
        }
        
        if (_timeAtEnd > 0)
            yield return (_startPos, _startRot, _loopTime - _timeAtEnd);

        yield return (_startPos, _startRot, _loopTime);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_startPos, 0.2f);

        Vector3 lastPos = _startPos;

        foreach ((Vector3 pos, Quaternion rot, int t) in AllPositions())
        {
            Gizmos.DrawLine(lastPos, pos);
            Gizmos.DrawWireSphere(pos, 0.2f);
            
            lastPos = pos;
        }
        
        Gizmos.DrawWireSphere(transform.position, 0.2f);

    }
}
