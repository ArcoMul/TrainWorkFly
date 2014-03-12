using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LearnBuilding : Building
{
	public CircleBar CircleBar;

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
		foreach (Worker w in Workers)
		{
			float timetolearn = TimeToLearnSkill * GetWorkerSkill(w);
			float time = (float)DateTime.Now.Subtract(WorkerTimeOfArrival[index]).TotalSeconds;
			CircleBar.SetPercentage(time / timetolearn);

			if(time >= timetolearn)
			{
				CircleBar.SetPercentage(0);
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

	private int GetWorkerSkill (Worker w) {
		if (Type == LearnType.Repair) {
			return w.RepairSkill;
		} else if (Type == LearnType.Paint) {
			return w.PaintSkill;
		}
		return 1;
	}
}
