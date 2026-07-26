using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BombCharacter))]
public class ImpendingExplosionVisual : MonoBehaviour
{
	[Header("Object to Affect")]
	[SerializeField] private GameObject _gfxObject;
	
	[Tooltip("When there's % amount of time left, trigger visual")]
	[SerializeField, Range(0, 1)] private float _whenToShake;
	[Tooltip("When there's % amount of time left, trigger visual")]
	[SerializeField, Range(0, 1)] private float _whenToScale;
	
	[Header("Design Values")] 
	[SerializeField] private float _scaleMultiplier;
	[SerializeField] private float _shakeIntensity;
	
	private BombCharacter _bombCharacter;

	private void Start()
	{
		_bombCharacter = GetComponent<BombCharacter>();

		float remainingTimeInSeconds = _bombCharacter.RemainingTime * 0.1f;
		
		StartCoroutine(Co_ShakeVisual(remainingTimeInSeconds *  _whenToShake));
		StartCoroutine(Co_ScaleVisual(remainingTimeInSeconds * _whenToScale));
	}
	
	private IEnumerator Co_ShakeVisual(float whenToShakeInSeconds)
	{
		Debug.Log($"Shaking in {whenToShakeInSeconds}");
		
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToShakeInSeconds);
		
		_gfxObject.transform.DOShakePosition(0.1f, new Vector3(_shakeIntensity, 0, _shakeIntensity), 10, 0.2f)
			.SetEase(Ease.InOutElastic)
			.SetLoops(-1, LoopType.Yoyo)
			.SetRelative()
			.SetLink(gameObject);
	}

	private IEnumerator Co_ScaleVisual(float whenToScaleInSeconds)
	{
		Debug.Log($"Scaling in {whenToScaleInSeconds}");
		
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToScaleInSeconds);
		
		_gfxObject.transform.DOScale(Vector3.one * _scaleMultiplier, whenToScaleInSeconds)
			.SetEase(Ease.OutCirc)
			.SetLink(gameObject);
	}
}
