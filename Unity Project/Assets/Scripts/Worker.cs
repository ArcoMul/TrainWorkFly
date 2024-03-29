﻿using UnityEngine;
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
	public enum States {Idle = 1, WalkingToBuilding = 2, WalkingFromBuilding = 3, Learning = 4, Working = 5, WalkingToPlayer = 6}
	public States State = States.Idle;
    public bool IsWorkingExtraHard;
    
    public GameObject TextCloud;
    private Transform Text1;
    private Transform Text2;
    private Transform Icon1;
    private Transform Icon2;


	/**
	 * What is the goal to walk to for this worker
	 */
	private Building WalkGoal;

	/**
	 * Walk to position
	 */
	private Vector3 WalkToPosition;
	private Vector3 SpawnPosition;

	public int PaintSkill;
	public int RepairSkill;

	public Skill[] Skills = new Skill[2];

	void Start ()
	{
        TextCloud = transform.FindChild("Textcloud").gameObject;
        TextCloud.SetActive(false);

        Text1 = TextCloud.transform.FindChild("Text1");
        Text2 = TextCloud.transform.FindChild("Text2");
        Icon1 = TextCloud.transform.FindChild("Icon1");
        Icon2 = TextCloud.transform.FindChild("Icon2");

		SpawnPosition = transform.position;

		// We start in Idle
		SwitchState (States.Idle);

		Boss.Instance.MakeWorkerJumpToIdle(this);
	}

	void Update ()
	{
		// On walking, walk to the goal
		if ((State == States.WalkingToBuilding || State == States.WalkingFromBuilding) && WalkGoal != null)
		{
            //Checking if the workers walks towards an taskbuilding that is not investigated yet
            if(State == States.WalkingToBuilding)
            {
                TaskBuilding taskbuilding = WalkGoal.GetComponent<TaskBuilding>();
                if (taskbuilding != null)
                {
                    if (!taskbuilding.Investigated)
                    {
                        SwitchState(States.WalkingToPlayer);
                        return;
                    }
                }

                LearnBuilding learnbuilding = WalkGoal.GetComponent<LearnBuilding>();
                if (learnbuilding != null)
                {
                    if (learnbuilding.WorkerItems.Count >= 1)
                    {
                        SwitchState(States.WalkingToPlayer);
                        return;
                    }
                }
            }

			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = GetWalkToPosition(State) - transform.position;
			Direction = new Vector3(Direction.x, Direction.y, 0);

			// Calculate the movement for this frame and add this to the position
			Vector3 Movement = Direction.normalized * Time.deltaTime * 3;
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
                    SwitchState(States.WalkingToPlayer);
				}
			}
        }
		else if (State == States.WalkingToPlayer || State == States.Idle)
        {
			if (Vector3.Distance(Boss.Instance.GetWorkerIdlePlace(this), transform.position) > 0.1f)
			{
				Vector3 Direction = Boss.Instance.GetWorkerIdlePlace(this) - transform.position;
                Direction = new Vector3(Direction.x, Direction.y, 0);

                // Calculate the movement for this frame and add this to the position
                Vector3 Movement = Direction.normalized * Time.deltaTime * 3;
                transform.position += Movement;
            }
			else if (State != States.Idle)
			{
            	SwitchState(States.Idle);
			}
        }
        else if (State == States.Working)
        {
            IsWorkingExtraHard =  (Vector3.Distance(Boss.Instance.transform.position, this.transform.position) < 1f);
            this.GetComponent<Animator>().enabled = IsWorkingExtraHard;
            if (!IsWorkingExtraHard)
            {
                this.transform.localScale = Vector3.one;
            }
        }
	}

	/**
	 * Switch from state and possibility to do some preperation for 
	 * this state
	 */
	public void SwitchState (States state) 
	{
		// Check if we switch from idle to something else
		if (State == States.Idle && state != States.Idle)
		{
			Boss.Instance.RemoveIdleWorker(this);
		}
		// Make sure the extra hard working animator is disabled when switching to not-working
		else if (State == States.Working && state != States.Working) 
		{
			if (this.GetComponent<Animator>() != null) {
				this.GetComponent<Animator>().enabled = false;
				// this.GetComponent<Animator>().animation["WorkerAnim"].time = 0;
			}
		}

		// Save the new state
		State = state;

		if (State == States.Idle)
		{
			Boss.Instance.AddIdleWorker(this);
		}
		else if (State == States.WalkingToBuilding)
		{
            if (WalkGoal != null)
            {
                WalkGoal.RemoveWorker(this);
            }
		}
		else if (State == States.WalkingFromBuilding)
		{
            // Nothing?
		}
		else if (State == States.Learning)
		{
			WalkGoal.AddWorker(this);
		}
		else if (State == States.Working)
		{
			WalkGoal.AddWorker(this);
		}
        else if (State == States.WalkingToPlayer)
        {

        }
    }

	public Vector3 GetWalkToPosition (States state) 
	{
        Vector3 result = Vector3.zero;

		State = state;
		if (State == States.Idle){
			result = SpawnPosition;
		}else if (State == States.WalkingToBuilding){
            if (WalkGoal != null)
            {
                result = WalkGoal.GetRestPosition();
            }
            else
            {
                Debug.LogError("WalkGoal is null, shouldn't happen!");
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
        // is null first time? - marijn
        if (WalkGoal != null)
        {
            WalkGoal.RemoveWorker(this);
        }
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

    public void OnMouseEnter()
    {
        TextCloud.SetActive(true);
        UpdateTextCloud();
    }

    public void OnMouseExit()
    {
        TextCloud.SetActive(false);
    }

    public void UpdateTextCloud()
    {
        if (Skills[0] != null)
        {
            Text1.gameObject.SetActive(true);
            Text1.GetComponent<TextMesh>().text = (Skills[0].Level - 1).ToString();

            Icon1.gameObject.SetActive(true);
            Icon1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Skills[0].Type.ToString());
        }
        else
        {
            Text1.gameObject.SetActive(false);
            Icon1.gameObject.SetActive(false);
        }

        if (Skills[1] != null)
        {
            Text2.gameObject.SetActive(true);
            Text2.GetComponent<TextMesh>().text = (Skills[1].Level - 1).ToString();

            Icon2.gameObject.SetActive(true);
            Icon2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Skills[1].Type.ToString());
        }
        else
        {
            Text2.gameObject.SetActive(false);
            Icon2.gameObject.SetActive(false);
        }
    }
}
