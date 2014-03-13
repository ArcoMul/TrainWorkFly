using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LearnBuilding : Building
{
	public CircleBar CircleBar;

	public float TimeToLearnSkill;
	public enum LearnType {Paint = 1, Repair = 2}
	public Skill.Types Type;
	
	void Update()
	{
		List<int> removedindexes = new List<int>();
		int index = 0;
		foreach (WorkerItem workerItem in WorkerItems)
		{
            Worker w = workerItem.Worker;
			float timetolearn = TimeToLearnSkill * w.GetLevel(Type);
            float time = (float)DateTime.Now.Subtract(workerItem.WorkerTimeOfArrival).TotalSeconds;
			CircleBar.SetPercentage(time / timetolearn);

			if(time >= timetolearn)
			{
				w.SwitchState(Worker.States.WalkingToPlayer);
				w.AddLevel (Type);
				CircleBar.SetPercentage(0);
				removedindexes.Add(index);
			}

			index++;
		}

		foreach (int i in removedindexes) {
            WorkerItems.RemoveAt(i);
		}
	}
}
