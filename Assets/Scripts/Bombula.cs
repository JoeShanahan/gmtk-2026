using UnityEngine;

public class Bombula : MonoBehaviour
{
	[SerializeField] private Animator _animator;

	private LevelManager _levelManager;
	
	private void Start()
	{
		_levelManager = FindAnyObjectByType<LevelManager>();
		_levelManager.OnLevelComplete += HandleLevelComplete;
	}

	private void OnDisable()
	{
		_levelManager.OnLevelComplete -= HandleLevelComplete;
	}

	private void HandleLevelComplete()
	{
		_animator.SetBool("isHit", true);
	}
}
