using System;
using UnityEngine;

/// <summary>
/// 3D one
/// </summary>
public class ExplosionIndicator : MonoBehaviour
{
	public Rigidbody RBToTrack { get; private set; }

	public void Init(Rigidbody rbToTrack)
	{
		RBToTrack = rbToTrack;
	}
	
	public void UpdateIndicator(ExplosionBase.PotentialExplosionData data)
	{
		if(data.Target != RBToTrack){ Debug.Log("Indicator trying to represent wrong object");}
		
		// set position
		transform.position = data.Target.transform.position;
		
		// set rotation 
		var rotation = transform.rotation;
		rotation.eulerAngles = GetEulerAnglesFromExplosionVector(data.ExplosionDirection);
		transform.rotation = rotation;
	}

	private Vector3 GetEulerAnglesFromExplosionVector(Vector3 explosionDirectionOnTarget)
	{
		Debug.Log($"My explosion vector is {explosionDirectionOnTarget}");
    
		// Create a quaternion that points toward the explosion direction
		Quaternion rotation = Quaternion.LookRotation(explosionDirectionOnTarget, Vector3.up);
    
		// Convert to euler angles
		Vector3 eulerAngles = rotation.eulerAngles;
    
		// Keep Z at 0, preserve X and Y
		return new Vector3(eulerAngles.x, eulerAngles.y, 0);
	}
	
}
