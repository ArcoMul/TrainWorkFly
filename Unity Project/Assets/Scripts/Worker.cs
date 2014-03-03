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
	public enum States {Idle = 1, WalkingToBuilding = 2, WalkingFromBuilding = 3, Learning = 4, Working = 5}
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

	/**
	 * Walk to position
	 */
	private Vector3 WalkToPosition;
	private Vector3 SpawnPosition;

	public LearnBar LearnBar;

	public TextMesh PaintText;
	public TextMesh RepairText;

	public int PaintSkill;
	public int RepairSkill;

	void Start ()
	{
		SpawnPosition = transform.position;
		// We start in Idle
		SwitchState (States.Idle);
		LearnBar.gameObject.SetActive(false);
	}

	void Update ()
	{
		// On Idle move around a bit to make the scene look alive
		if (State == States.Idle)
		{
			transform.position = StartIdlePosition - new Vector3(2.5f * Time.deltaTime, 2.5f * Time.deltaTime, 0) + new Vector3(Random.value * 5f * Time.deltaTime, Random.value * 5f * Time.deltaTime, 0);
		}
		// On walking, walk to the goal
		else if (State == States.WalkingToBuilding || State == States.WalkingFromBuilding)
		{
			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = WalkToPosition - transform.position;
			Direction = new Vector3(Direction.x, Direction.y, 0);

			// Calculate the movement for this frame and add this to the position
			Vector3 Movement = Direction.normalized * Time.deltaTime * 2;
			transform.position += Movement;

			// If the movement is bigger than the actual length to walk in total, switch to idle
			//  (-0.01f is a small margin to make sure we always switch)
			if (Mathf.Abs(Movement.x) > Mathf.Abs(Direction.x) - 0.01f && Mathf.Abs(Movement.y) > Mathf.Abs(Direction.y) - 0.01f) {
				if(State == States.WalkingToBuilding){
					if (WalkGoal.GetComponent<LearnBuilding>() != null) {
						SwitchState (States.Learning);
					} else if (WalkGoal.GetComponent<TaskBuilding>() != null) {
						SwitchState (States.Working);
					}
				} else if(State == States.WalkingFromBuilding){
					SwitchState (States.Idle);
				}
			}
		}
	}

	/**
	 * Switch from state and possibility to do some preperation for 
	 * this state
	 */
	public void SwitchState (States state) 
	{
		State = state;
		if (State == States.Idle)
		{
			StartIdlePosition = transform.position;
		}
		else if (State == States.WalkingToBuilding)
		{
			WalkGoal.walkingWorkers += 1;
			WalkToPosition = WalkGoal.GetRestPosition ();
		}
		else if (State == States.WalkingFromBuilding)
		{
			LearnBar.gameObject.SetActive(false);
			WalkToPosition = SpawnPosition;
		}
		else if (State == States.Learning)
		{
			StartIdlePosition = transform.position;
			WalkGoal.AddWorker(this);
			LearnBar.gameObject.SetActive(true);
		}
		else if (State == States.Working)
		{
			StartIdlePosition = transform.position;
			WalkGoal.AddWorker(this);
		}
    }

	/**
	 * New goal for the worker, save the goal and switch to walking
	 */
	public void SetGoal (Building building)
	{
		WalkGoal = building;
		SwitchState (States.WalkingToBuilding);
	}

	public void AddPaintLevel(int amount)
	{
		PaintSkill += amount;
		PaintText.text = "Paint: " + PaintSkill;
	}

	public void AddRepairLevel(int amount)
	{
		RepairSkill += amount;
		RepairText.text = "Repair: " + RepairSkill;
	}
	
}
