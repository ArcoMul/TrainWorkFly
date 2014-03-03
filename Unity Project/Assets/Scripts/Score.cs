using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	private static Score _Instance = null;
	public static Score Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = FindObjectOfType<Score>();
			return _Instance;
		}
	}

	public TextMesh Text;

	private int _Points = 0;
	public int Points {
		get {
			return _Points;
		}
		set {
			_Points = value;
			Text.text = "Score: " + _Points;
		}
	}
}
