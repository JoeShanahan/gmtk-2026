using System;
using UnityEngine;

/// <summary>
/// 3D one
/// </summary>
public class ExplosionIndicator : MonoBehaviour
{
	public Rigidbody RBToTrack { get; private set; }

	[SerializeField] private GameObject _gfx;
	[SerializeField] private Material _material;
	
	[Header("Weak Visuals")] 
	[SerializeField] private Color _weakColour;
	[SerializeField] private float _weakScaleMultiplier;
	
	[Header("Strong Visuals")] 
	[SerializeField] private Color _strongColour;
	[SerializeField] private float _strongScaleMultiplier;

	private float _cachedStartingScale;
	private Material _materialInstance;

	public void Init(Rigidbody rbToTrack)
	{
		RBToTrack = rbToTrack;

		_cachedStartingScale = _gfx.transform.localScale.x;
		_materialInstance = _gfx.GetComponent<MeshRenderer>().material;
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
		
		// set strength visual 
		SetStrengthVisual(data.IsWeakRange);
	}

	private Vector3 GetEulerAnglesFromExplosionVector(Vector3 explosionDirectionOnTarget)
	{
		Quaternion rotation = Quaternion.LookRotation(explosionDirectionOnTarget, Vector3.up);
		Vector3 eulerAngles = rotation.eulerAngles;
    
		// keep z at 0
		return new Vector3(eulerAngles.x, eulerAngles.y, 0);
	}

	private void SetStrengthVisual(bool isWeakRange)
	{
		float scaleMultiplier = isWeakRange ? _weakScaleMultiplier : _strongScaleMultiplier;
		_gfx.transform.localScale = Vector3.one * (scaleMultiplier * _cachedStartingScale);

		_materialInstance.color = isWeakRange ? _weakColour : _strongColour;
	}

	private void OnDestroy()
	{
		Destroy(_materialInstance);
	}
}
