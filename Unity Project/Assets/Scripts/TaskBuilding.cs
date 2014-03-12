

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TaskBuilding : Building
{
	public enum States {Idle = 1, Moving = 2}
	public States State = States.Idle;

	public Skill.Types Type;

	public CircleBar ProgressBar;

	private DateTime SpawnTime;

    public double TotalTimeWorkedOnTask = 0;

    public bool IsCompleted = false;

	protected virtual void Start ()
	{
		base.Start();

		SpawnTime = DateTime.Now;

		// Hide the progressbar
		ProgressBar.gameObject.SetActive(false);
	}

	void Update()
	{
		// If nobody is working, dont mind this
		if (Workers.Count == 0) return;

		// Calculate how much time there is worked on the task		
		int i = 0;
		foreach (Worker w in Workers) {
            float increaserForHardWorkers = w.IsWorkingExtraHard ? 2.5f : 1f;
            float increaserSoItWontBeSoSlow = 1.5f;
            TotalTimeWorkedOnTask += Time.deltaTime * increaserSoItWontBeSoSlow * w.GetLevel(this.Type) * increaserForHardWorkers;
			i++;
		}

		// Stop if there is not worked on the task
		if (TotalTimeWorkedOnTask == 0) return;

		// Make progressbar active if there is build for a bit
		if (!ProgressBar.gameObject.activeSelf) {
			ProgressBar.gameObject.SetActive(true);
		}

		// Update progressbar with new value
		ProgressBar.SetPercentage((float) TotalTimeWorkedOnTask / Airplane.CurrentAirplane.TimeToFinishPlane );

		// If the worked time is bigger than than the needed time to finish the task, dismiss the task and workers
        if (TotalTimeWorkedOnTask >= Airplane.CurrentAirplane.TimeToFinishPlane)
		{
			FinishTask();
		}
	}

	private void FinishTask ()
	{
		foreach (Worker w in Workers) {
			w.SwitchState(Worker.States.WalkingFromBuilding);
		}
        IsCompleted = true;
		Score.Instance.Points++;

		Destroy(gameObject);
	}

	private void FailTask ()
	{
		Destroy(gameObject);
		Application.LoadLevel("GameOver");
	}
}
