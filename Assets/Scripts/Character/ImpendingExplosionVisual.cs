using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BombCharacter))]
[RequireComponent(typeof(ExplosionBase))]
public class ImpendingExplosionVisual : MonoBehaviour
{
	[Header("Object to Affect")]
	[SerializeField] private GameObject _gfxObject;
	[SerializeField] private SkinnedMeshRenderer _bodyRenderer;
	[SerializeField] private LineRenderer _weakExplosionRange;
	[SerializeField] private LineRenderer _strongExplosionRange;
	
	[Header("Timings")]
	[Tooltip("When there's % amount of time left, trigger shake")]
	[SerializeField, Range(0, 1)] private float _whenToShake;
	[Tooltip("When there's % amount of time left, trigger scale")]
	[SerializeField, Range(0, 1)] private float _whenToScale;
	[Tooltip("When there's % amount of time left, show range")]
	[SerializeField, Range(0, 1)] private float _whenToShowRange;
	[Tooltip("When there's % amount of time left, start to ramp wobble")]
	[SerializeField, Range(0, 1)] private float _whenToWobble;
	
	[Header("Design Values")]
	[SerializeField] private float _scaleMultiplier;
	[SerializeField] private float _shakeIntensity;
	[SerializeField] private float _targetWobbleIntensity = 0.1f;
	
	private BombCharacter _bombCharacter;
	private ExplosionBase _explosionBase;
	
	private void Start()
	{
		_bombCharacter = GetComponent<BombCharacter>();
		_explosionBase = GetComponent<ExplosionBase>();

		float remainingTimeInSeconds = _bombCharacter.RemainingTime * 0.1f;
		StartCoroutine(Co_ShakeVisual(remainingTimeInSeconds *  _whenToShake));
		StartCoroutine(Co_ScaleVisual(remainingTimeInSeconds * _whenToScale));
		StartCoroutine(Co_ExplosionRadius(remainingTimeInSeconds * _whenToShowRange));
		StartCoroutine(Co_Wobble(remainingTimeInSeconds * _whenToWobble));
	}
	
	private IEnumerator Co_ShakeVisual(float whenToShakeInSeconds)
	{
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToShakeInSeconds);
		
		_gfxObject.transform.DOShakePosition(0.1f, new Vector3(_shakeIntensity, 0, _shakeIntensity), 10, 0.2f)
			.SetEase(Ease.InOutElastic)
			.SetLoops(-1, LoopType.Yoyo)
			.SetRelative()
			.SetLink(gameObject);
	}

	private IEnumerator Co_ScaleVisual(float whenToScaleInSeconds)
	{
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToScaleInSeconds);
		
		_gfxObject.transform.DOScale(Vector3.one * _scaleMultiplier, whenToScaleInSeconds)
			.SetEase(Ease.OutCirc)
			.SetLink(gameObject);
	}

	private IEnumerator Co_ExplosionRadius(float whenToFlashRange)
	{
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToFlashRange);

		while (_bombCharacter.RemainingTime > 0)
		{
			_explosionBase.ShowExplosionRadius(_weakExplosionRange, _strongExplosionRange);
			yield return null;
		}
	}

	private IEnumerator Co_Wobble(float whenToWobble)
	{
		yield return new WaitUntil(() => _bombCharacter.RemainingTime * 0.1f <= whenToWobble);
		Material instanceMaterial = _bodyRenderer.material;
		string property = "_Wobble_Amount";

		DOTween.To(
				() => instanceMaterial.GetFloat(property),
				x => instanceMaterial.SetFloat(property, x),
				_targetWobbleIntensity,
				_bombCharacter.RemainingTime * 0.1f
			)
			.SetLink(gameObject)
			.SetEase(Ease.OutCirc);
	}
}