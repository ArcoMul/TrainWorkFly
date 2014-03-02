using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour
{
	public enum States {Idle = 1, Walking = 2}
	public States State = States.Idle;

	private Vector3 StartIdlePosition;

	private Building Goal;

	void Start ()
	{
		SwitchState (States.Idle);
	}

	void Update ()
	{
		if (State == States.Idle)
		{
			transform.position = StartIdlePosition - new Vector3(2.5f * Time.deltaTime, 2.5f * Time.deltaTime, 0) + new Vector3(Random.value * 5f * Time.deltaTime, Random.value * 5f * Time.deltaTime, 0);
		}
		else if (State == States.Walking)
		{
			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = Goal.GetRestPosition () - transform.position;
			Direction = new Vector3(Direction.x, Direction.y, 0);

			// Calculate the movement for this frame and add this to the position
			Vector3 Movement = Direction.normalized * Time.deltaTime * 2;
			transform.position += Movement;

			// If the movement is bigger than the actual length to walk in total, switch to idle
			//  (-0.01f is a small margin to make sure we always switch)
			if (Movement.x > Direction.x - 0.01f && Movement.y > Direction.y - 0.01f) {
				SwitchState (States.Idle);
			}
		}
	}

	public void SetGoal (Building building)
	{
		Goal = building;
		SwitchState (States.Walking);
	}

	void SwitchState (States state) 
	{
		State = state;
		if (State == States.Idle) {
			StartIdlePosition = transform.position;
		}
	}
}
