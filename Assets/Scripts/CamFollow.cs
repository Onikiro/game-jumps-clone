using UnityEngine;

public class CamFollow : MonoBehaviour
{
	[SerializeField] private Transform _player;

	private bool _inGame;

	private void OnEnable()
	{
		_inGame = true;
		Controller.OnGameOver += () => _inGame = false;
	}

	private void Awake()
	{
		Application.targetFrameRate = 60;
		Screen.orientation = ScreenOrientation.Portrait;
	}

	private void LateUpdate()
	{
		if(_inGame)
		transform.position = new Vector3(_player.position.x - 3, transform.position.y, _player.position.z - 3);
	}
}