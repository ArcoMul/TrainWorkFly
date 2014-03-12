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

	public Skill[] Skills = new Skill[2];

	void Start ()
	{
		SpawnPosition = transform.position;

		// We start in Idle
		SwitchState (States.Idle);
		LearnBar.gameObject.SetActive(false);
	}

	void Update ()
	{
		// On walking, walk to the goal
		if (State == States.WalkingToBuilding || State == States.WalkingFromBuilding)
		{
			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = GetWalkToPosition(State) - transform.position;
			Direction = new Vector3(Direction.x, Direction.y, 0);

			// Calculate the movement for this frame and add this to the position
			Vector3 Movement = Direction.normalized * Time.deltaTime * 2;
			transform.position += Movement;

			// If the movement is bigger than the actual length to walk in total, switch to idle
			//  (-0.01f is a small margin to make sure we always switch)
			if (Mathf.Abs(Movement.x) > Mathf.Abs(Direction.x) - 0.01f && Mathf.Abs(Movement.y) > Mathf.Abs(Direction.y) - 0.01f) {
				if(State == States.WalkingToBuilding && WalkGoal != null){
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
		}
		else if (State == States.WalkingToBuilding)
		{
			WalkGoal.WalkingWorkers++;
		}
		else if (State == States.WalkingFromBuilding)
		{
			WalkGoal.WalkingWorkers--;
			LearnBar.gameObject.SetActive(false);
		}
		else if (State == States.Learning)
		{
			WalkGoal.AddWorker(this);
		}
		else if (State == States.Working)
		{
			WalkGoal.AddWorker(this);
		}
    }

	public Vector3 GetWalkToPosition (States state) 
	{
		Vector3 result = new Vector3(0.0f, 0.0f, 0.0f);

		State = state;
		if (State == States.Idle){
			result = SpawnPosition;
		}else if (State == States.WalkingToBuilding){
			if(WalkGoal != null){
				result =  WalkGoal.GetRestPosition ();
			}
		}else if (State == States.WalkingFromBuilding){
			result =  SpawnPosition;
		}else if (State == States.Learning){
			result =  transform.position;
		}else if (State == States.Working){
			result =  transform.position;
		}

		return result;
	}
	
	/**
	 * New goal for the worker, save the goal and switch to walking
	 */
	public void SetGoal (Building building)
	{
		WalkGoal = building;
		SwitchState (States.WalkingToBuilding);
	}

	public void AddLevel (Skill.Types Type)
	{
		if (Skills[0] != null && Skills[0].Type == Type)
		{
			Skills[0].Level++;
		}
		else if (Skills[1] != null && Skills[1].Type == Type)
		{
			Skills[1].Level++;
		}
		else if (Skills[0] == null) {
			Skills[0] = new Skill (Type);
		} else if (Skills[1] == null) {
			Skills[1] = new Skill (Type);
		} else if (Skills[0].Level <= Skills[1].Level) {
			Skills[0] = new Skill (Type);
		} else {
			Skills[1] = new Skill (Type);
		}

		if (Skills[0] != null) {
			PaintText.text = Skills[0].Type.ToString() + ": " + (Skills[0].Level - 1);
		}
		if (Skills[1] != null) {
			RepairText.text = Skills[1].Type.ToString() + ": " + (Skills[1].Level - 1);
		}
	}

	public int GetLevel (Skill.Types Type)
	{
		if (Skills[0] != null && Skills[0].Type == Type) {
			return Skills[0].Level;
		} else if (Skills[1] != null && Skills[1].Type == Type) {
			return Skills[1].Level;
		}
		return 1;
	}

}
