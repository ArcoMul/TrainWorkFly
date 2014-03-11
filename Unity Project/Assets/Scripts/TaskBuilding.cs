using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TaskBuilding : Building
{
	public enum States {Idle = 1, Moving = 2}
	public States State = States.Idle;

	public enum TaskTypes {Repair, Paint}
	public TaskTypes TaskType;

	public float TimeToFinishTask;
	public CircleBar ProgressBar;

	public int TimeToFailTask;
	public LearnBar FailBar;

	private DateTime SpawnTime;

	public TaskManager Manager;

	public Vector3 MoveToPosition;

    public double TotalTimeWorkedOnTask = 0;

	protected virtual void Start ()
	{
		base.Start();

		SpawnTime = DateTime.Now;

		// Hide the progressbar
		ProgressBar.gameObject.SetActive(false);
	}

	void Update()
	{
		if (State == States.Moving)
		{
			// Get the direction from the worker to the building and reset the z axis
			Vector3 Direction = MoveToPosition - transform.position;
			Direction = new Vector3(Direction.x, Direction.y, 0);
			
			// Calculate the movement for this frame and add this to the position
			Vector3 Movement = Direction.normalized * Time.deltaTime * 2;
			transform.position += Movement;
			
			// If the movement is bigger than the actual length to walk in total, switch to idle
			//  (-0.01f is a small margin to make sure we always switch)
			if (Mathf.Abs(Movement.x) > Mathf.Abs(Direction.x) - 0.01f && Mathf.Abs(Movement.y) > Mathf.Abs(Direction.y) - 0.01f) {
				State = States.Idle;
			}
		}
		double TimeSpanSinceSpawn = DateTime.Now.Subtract(SpawnTime).TotalSeconds;
		FailBar.SetPercentage((float) TimeSpanSinceSpawn / TimeToFailTask );

		if (TimeSpanSinceSpawn > TimeToFailTask) {
			FailTask();
			return;
		}

		// If nobody is working, dont mind this
		if (Workers.Count == 0) return;

		// Calculate how much time there is worked on the task
		
		int i = 0;
		foreach (Worker w in Workers) {
            float increaserForHardWorkers = w.IsWorkingExtraHard ? 2.5f : 1f;
            float increaserSoItWontBeSoSlow = 1.5f;
            TotalTimeWorkedOnTask += Time.deltaTime * increaserSoItWontBeSoSlow * GetWorkerSkill(w) * increaserForHardWorkers;
			i++;
		}
        Debug.Log("TotalTimeWorkedOnTask: " + TotalTimeWorkedOnTask);
		// Stop if there is not worked on the task
		if (TotalTimeWorkedOnTask == 0) return;

		// Make progressbar active if there is build for a bit
		if (!ProgressBar.gameObject.activeSelf) {
			ProgressBar.gameObject.SetActive(true);
		}

		// Update progressbar with new value
		ProgressBar.SetPercentage((float) TotalTimeWorkedOnTask / TimeToFinishTask );

		// If the worked time is bigger than than the needed time to finish the task, dismiss the task and workers
		if(TotalTimeWorkedOnTask >= TimeToFinishTask)
		{
			FinishTask();
		}
	}

	private int GetWorkerSkill (Worker w) {
		if (TaskType == TaskTypes.Repair) {
			return w.RepairSkill;
		} else if (TaskType == TaskTypes.Paint) {
			return w.PaintSkill;
		}
		return 1;
	}

	private void FinishTask ()
	{
		foreach (Worker w in Workers) {
			w.SwitchState(Worker.States.WalkingFromBuilding);
		}
		Manager.UpdateOnFinishTask ();
		Score.Instance.Points++;

		Destroy(gameObject);
	}

	private void FailTask ()
	{
		Destroy(gameObject);
		Application.LoadLevel("GameOver");
	}
}
