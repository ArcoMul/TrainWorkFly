using UnityEngine;
using System.Collections;

public class LearnBar : MonoBehaviour {
	public GameObject GreenBar;
	private float StartScale;

	// Use this for initialization
	void Awake () {
		StartScale = GreenBar.transform.localScale.x;

		Vector3 scale = GreenBar.transform.localScale;
		scale.x = 0.0f;
		GreenBar.transform.localScale = scale;
	}

	public void SetPercentage(float percentage)
	{
		GreenBar.transform.localScale = new Vector3 (percentage * StartScale, GreenBar.transform.localScale.y, GreenBar.transform.localScale.z);
	}
}
