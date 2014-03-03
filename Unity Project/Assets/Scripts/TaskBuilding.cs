using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TaskBuilding : Building
{
	public enum TaskTypes {Repair, Paint}
	public TaskTypes TaskType;

	public float TimeToFinishTask;
	public LearnBar ProgressBar;

	protected virtual void Start ()
	{
		base.Start();

		// Hide the progressbar
		ProgressBar.gameObject.SetActive(false);
	}

	void Update()
	{
		// If nobody is working, dont mind this
		if (Workers.Count == 0) return;

		// Calculate how much time there is worked on the task
		double TotalTimeWorkedOnTask = 0;
		int i = 0;
		foreach (Worker w in Workers) {
			TotalTimeWorkedOnTask += DateTime.Now.Subtract(WorkerTimeOfArrival[i]).TotalSeconds * GetWorkerSkill(w);
			i++;
		}

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
			foreach (Worker w in Workers) {
				w.SwitchState(Worker.States.WalkingFromBuilding);
			}
			Destroy(gameObject);
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
}
