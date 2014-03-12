using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour
{

	void Start ()
	{
        Score.Instance.UpdateText();
	}
}
