using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	private Text score;

	[SerializeField] private Transform _player;
	
	private void Start()
	{
		score = GetComponent<Text>();
	}

	void Update ()
	{
		score.text = "Score: " + (int)_player.position.x + "m.";
	}
}
