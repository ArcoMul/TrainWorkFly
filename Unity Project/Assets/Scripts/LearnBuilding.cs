﻿using UnityEngine;
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
		foreach (Worker w in Workers)
		{
			float timetolearn = TimeToLearnSkill * w.GetLevel(Type);
			float time = (float)DateTime.Now.Subtract(WorkerTimeOfArrival[index]).TotalSeconds;
			CircleBar.SetPercentage(time / timetolearn);

			if(time >= timetolearn)
			{
				w.SwitchState(Worker.States.WalkingFromBuilding);
				w.AddLevel (Type);
				CircleBar.SetPercentage(0);
				removedindexes.Add(index);
			}

			index++;
		}

		foreach (int i in removedindexes) {
			Workers.RemoveAt(i);
			WorkerTimeOfArrival.RemoveAt(i);
		}
	}
}
