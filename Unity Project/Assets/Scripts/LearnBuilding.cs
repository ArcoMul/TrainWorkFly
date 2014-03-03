using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LearnBuilding : Building {
	public float TimeToLearnSkill;
	public enum LearnType {Paint = 1, Repair = 2}
	public LearnType Type;
	
	// Use this for initialization
	protected override void Start () {
		base.Start (); 
	}

	void Update()
	{
		List<int> removedindexes = new List<int>();
		int index = 0;
		foreach (Worker w in Workers) {
			float time = (float)DateTime.Now.Subtract(WorkerTimeOfArrival[index]).TotalSeconds;
			w.LearnBar.SetPercentage(time / TimeToLearnSkill );

			if(time >= TimeToLearnSkill)
			{
				w.SwitchState(Worker.States.WalkingFromBuilding);
				removedindexes.Add(index);

				if(Type == LearnType.Paint) {
					w.AddPaintLevel(1);
				}else if(Type == LearnType.Repair) {
					w.AddRepairLevel(1);
				}
			}

			index++;
		}

		foreach (int i in removedindexes) {
			Workers.RemoveAt(i);
			WorkerTimeOfArrival.RemoveAt(i);
		}
	}
}
