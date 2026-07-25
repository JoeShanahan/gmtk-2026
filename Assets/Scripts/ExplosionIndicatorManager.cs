using System.Collections;
using UnityEngine;
using WorldToCanvas;

public class ExplosionIndicatorManager : MonoBehaviour
{
	[SerializeField] private GameObject _indicatorPrefab;
	[SerializeField] private GameObject _testFollower;

	private ExplosionIndicatorArrow _instance;

	private void Start()
	{
		_instance = W2CManager.InstantiateAs<ExplosionIndicatorArrow>(_indicatorPrefab);

		_instance.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		_instance.SetPosition(_testFollower.transform);
	}
}
