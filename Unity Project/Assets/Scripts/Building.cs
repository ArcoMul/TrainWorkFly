using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * A building is a place a worker can walk to, so far
 */
public class Building : MonoBehaviour
{
	public int WalkingWorkers;
	public List<Worker> Workers;
	public List<DateTime> WorkerTimeOfArrival;

	protected virtual void Start()
	{
		Workers = new List<Worker> ();
		WorkerTimeOfArrival = new List<DateTime> ();
	}

	/**
	 * On mouse down check if a worker is selected, if so give this building
	 * as goal for the worker. If not select the building.
	 */
	void OnMouseDown ()
	{
		Worker worker = null;
		if (Selecter.Instance.Active != null) {
			worker = Selecter.Instance.Active.GetComponent<Worker>();
		}
		if (worker == null)
		{
			Selecter.Instance.Select (GetComponent<Selectable>());
			return;
		}

		worker.SetGoal(this);
	}

	/**
	 * Returns the position where a worker can 'rest'
	 */
	public Vector3 GetRestPosition ()
	{
		return transform.position - new Vector3 ((Workers.Count * 0.6f) + 0.7f, 0, 0);
	}

	public void AddWorker(Worker worker)
	{
		Workers.Add (worker);
		WorkerTimeOfArrival.Add(DateTime.Now);
	}
}
