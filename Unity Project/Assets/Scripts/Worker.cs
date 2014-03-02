using UnityEngine;
using System.Collections;

/**
 * The worker can fix tasks and learn new skills. By learning new skills
 * he becomes better in certain tasks. To learn new skills and do his tasks
 * he can walk around the level
 */
public class Worker : MonoBehaviour
{
	/**
	 * Different states for the worker
	 */
	public enum States {Idle = 1, Walking = 2}
	public States State = States.Idle;

	/**
	 * The start position when going idle, the worker moves around a bit on
	 * this spot
	 */
	private Vector3 StartIdlePosition;

	/**
	 * What is the goal to walk to for this worker
	 */
	private Building WalkGoal;

	void Start ()
	{
		// We start in Idle
		SwitchState (States.Idle);
	}

	void Update ()
	{
		// On Idle move around a bit to make the scene look alive
		if (State == States.Idle)
		{
			transform.position = StartIdlePosition - new Vector3(2.5f * Time.deltaTime, 2.5f * Time.deltaTime, 0) + new Vector3(Random.value * 5f * Time.deltaTime, Random.value * 5f * Time.deltaTime, 0);
		}
		// On walking, walk to the goal
		else if (State == States.Walking)
		{
			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = WalkGoal.GetRestPosition () - transform.position;
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

	/**
	 * Switch from state and possibility to do some preperation for 
	 * this state
	 */
	void SwitchState (States state) 
	{
		State = state;
		if (State == States.Idle) {
			StartIdlePosition = transform.position;
        }
    }

	/**
	 * New goal for the worker, save the goal and switch to walking
	 */
	public void SetGoal (Building building)
	{
		WalkGoal = building;
		SwitchState (States.Walking);
	}
}
