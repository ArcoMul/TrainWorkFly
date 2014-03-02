using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
	public enum States {Idle = 1}
	public States State = States.Idle;

	private Vector3 StartPosition;

	void Start ()
	{
		StartPosition = transform.position;
	}

	void Update ()
	{
		if (State == States.Idle)
		{
			transform.position = StartPosition - new Vector3(2.5f * Time.deltaTime, 2.5f * Time.deltaTime, 0) + new Vector3(Random.value * 5f * Time.deltaTime, Random.value * 5f * Time.deltaTime, 0);
		}
	}

}
