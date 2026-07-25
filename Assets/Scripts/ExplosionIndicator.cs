using System;
using UnityEngine;

/// <summary>
/// 3D one
/// </summary>
public class ExplosionIndicator : MonoBehaviour
{
	[SerializeField] private GameObject _arrowGFX;
	
	public Rigidbody RBToTrack { get; private set; }
	public ExplosionBase ClosestExplosive { get; private set; }

	public void Init(Rigidbody rbToTrack, ExplosionBase explosive)
	{
		RBToTrack = rbToTrack;
		ClosestExplosive = explosive;
	}

	public void SetClosestExplosive(ExplosionBase newBase)
	{
		ClosestExplosive = newBase;
	}

	private void LateUpdate()
	{
		if (RBToTrack == null) return;
		
		// set position
		transform.position = RBToTrack.transform.position;
		
		// set rotation 
		var rotation = _arrowGFX.transform.rotation;
		rotation.eulerAngles = new Vector3(0, 0, GetZRotationAngle());
		_arrowGFX.transform.rotation = rotation;
	}

	private float GetZRotationAngle()
	{
		// needs to know closest bomb , and its explosion direction
		return 0;
	}
	
}
