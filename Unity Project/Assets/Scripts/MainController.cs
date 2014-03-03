using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour
{

	void Start ()
	{
		DontDestroyOnLoad(Score.Instance);
	}
	
}
