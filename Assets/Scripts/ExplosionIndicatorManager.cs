using System.Collections.Generic;
using UnityEngine;

public class ExplosionIndicatorManager : MonoBehaviour
{
	[SerializeField] private ExplosionIndicator _indicatorPrefab;
	
	private List<ExplosionIndicator> _activeExplosionIndicators = new List<ExplosionIndicator>();
	
	private void Update()
	{
		List<Rigidbody> allRigidbodiesInRange = new List<Rigidbody>();
		List<ExplosionBase.PotentialExplosionData> allPotentialExplosions = new List<ExplosionBase.PotentialExplosionData>();
		
		for (int i = 0; i < BombManager.Instance.AllBombs.Count; i++)
		{
			// get explosion bases
			if (!BombManager.Instance.AllBombs[i].TryGetComponent(out ExplosionBase explosionBase))
			{
				Debug.LogWarning($"Something probably went wrong, couldn't find explosion base on {BombManager.Instance.AllBombs[i]}");
			}
			
			ExplosionBase.PotentialExplosionData[] potentialExplosions = explosionBase.GetPotentialExplosions();

			for (int j = 0; j < potentialExplosions.Length; j++)
			{
				allPotentialExplosions.Add(potentialExplosions[j]);
				
				if(!allRigidbodiesInRange.Contains(potentialExplosions[j].Target)) allRigidbodiesInRange.Add(potentialExplosions[j].Target);
			}
		}

		// go through all RBs affected 
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
				newIndicator.Init(allRigidbodiesInRange[i]);

				indicatorOfRB = newIndicator;
			}
			
			// Update the indicator's data display
			indicatorOfRB.UpdateIndicator(GetClosestPotentialExplosion(allRigidbodiesInRange[i], allPotentialExplosions));
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

	private ExplosionBase.PotentialExplosionData GetClosestPotentialExplosion(Rigidbody targetRB, List<ExplosionBase.PotentialExplosionData> potentialExplosions)
	{
		ExplosionBase.PotentialExplosionData closestExplosion = new ExplosionBase.PotentialExplosionData();
		
		float closestDistance = float.MaxValue;
		Vector3 targetPosition = targetRB.transform.position;

		foreach (ExplosionBase.PotentialExplosionData potentialExplosion in potentialExplosions)
		{
			if(potentialExplosion.Target != targetRB) continue;
			
			// get the closest bomb to RB 
			float distance = Vector3.Distance(potentialExplosion.Bomb.transform.position, targetPosition);
        
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestExplosion = potentialExplosion;
			}
		}

		return closestExplosion;
	}
}
