using System.Collections.Generic;
using UnityEngine;

public class ExplosionIndicatorManager : MonoBehaviour
{
	[SerializeField] private ExplosionIndicator _indicatorPrefab;
	private List<ExplosionIndicator> _activeExplosionIndicators;
	
	private void Update()
	{
		List<Rigidbody> allRigidbodiesInRange = new List<Rigidbody>();
		List<ExplosionBase> allExplosionBases = new List<ExplosionBase>();
		
		for (int i = 0; i < BombManager.Instance.AllBombs.Count; i++)
		{
			// get explosion bases
			if (!BombManager.Instance.AllBombs[i].TryGetComponent(out ExplosionBase explosionBase))
			{
				Debug.LogWarning($"Something probably went wrong, couldn't find explosion base on {BombManager.Instance.AllBombs[i]}");
			}
			
			allExplosionBases.Add(explosionBase);
			
			// get bodies in range
			Rigidbody[] bodiesInRange = explosionBase.GetObjectsInRange();
			
			// keep track of all bodies so we can clean orphaned ones
			for (int j = 0; j < bodiesInRange.Length; j++)
			{
				if(!allRigidbodiesInRange.Contains(bodiesInRange[j])) allRigidbodiesInRange.Add(bodiesInRange[j]);
			}
		}

		for (int i = 0; i < allRigidbodiesInRange.Count; i++)
		{
			ExplosionIndicator indicatorOfRB = null;
			// check if they already have an indicator
			for (int j = 0; j < _activeExplosionIndicators.Count; j++)
			{
				if (_activeExplosionIndicators[j].RBToTrack == allRigidbodiesInRange[i])
				{
					indicatorOfRB = _activeExplosionIndicators[j];
					break;
				}
			}

			// means no indicator already assigned to this rb so create a new one
			if (indicatorOfRB == null)
			{
				ExplosionIndicator newIndicator = Instantiate(_indicatorPrefab, transform);
				_activeExplosionIndicators.Add(newIndicator);
				newIndicator.Init(allRigidbodiesInRange[i], GetClosestExplosionBase(allRigidbodiesInRange[i], allExplosionBases));

				break;
			}
			
			// check that the indicator assigned is using the closest explosive base
			indicatorOfRB.SetClosestExplosive(GetClosestExplosionBase(allRigidbodiesInRange[i], allExplosionBases));
		}

		// remove orphaned indicators
		for (int i = 0; i < _activeExplosionIndicators.Count; i++)
		{
			if (!allRigidbodiesInRange.Contains(_activeExplosionIndicators[i].RBToTrack))
			{
				Destroy(_activeExplosionIndicators[i].gameObject);
				_activeExplosionIndicators.Remove(_activeExplosionIndicators[i]);
			}
		}
	}

	private ExplosionBase GetClosestExplosionBase(Rigidbody rb, List<ExplosionBase> explosionBases)
	{
		ExplosionBase closestBomb = null;
		float closestDistance = float.MaxValue;
		Vector3 targetPosition = rb.transform.position;

		foreach (ExplosionBase bomb in explosionBases)
		{
			float distance = Vector3.Distance(bomb.transform.position, targetPosition);
        
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestBomb = bomb;
			}
		}

		return closestBomb;
	}
}
