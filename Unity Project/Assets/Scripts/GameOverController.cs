using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour
{
	public TextMesh ScoreText;

	void Start ()
	{
		ScoreText.text = "Score: " + Score.Instance.Points;
	}
}
