using UnityEngine;
using System.Collections;

public class CircleBar : MonoBehaviour
{
	public Sprite[] Frames;

	void Awake () {
	}
	
	public void SetPercentage(float percentage)
	{
		percentage = percentage > 1 ? 1 : percentage;
		GetComponent<SpriteRenderer>().sprite = Frames[(int) Mathf.Floor(percentage * (Frames.Length - 1))];
	}
}
