using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class WorkerItem
{
    public Worker Worker;
    public DateTime WorkerTimeOfArrival;
}
/**
 * A building is a place a worker can walk to, so far
 */
public class Building : MonoBehaviour
{
    public List<WorkerItem> WorkerItems;

    int Workers
    {
        get
        {
            return WorkerItems.Count;
        }
    }

	protected virtual void Start()
	{
        WorkerItems = new List<WorkerItem>();
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

    public virtual void Update()
    {
        //Debug.Log("Workers: " + WorkerItems.Count);
    }

	/**
	 * Returns the position where a worker can 'rest'
	 */
	public Vector3 GetRestPosition ()
	{
        return transform.position - new Vector3((WorkerItems.Count * 0.6f) + 0.7f, 0, 0);
	}

	public void AddWorker(Worker worker)
	{
        Debug.Log("Adding worker " + WorkerItems.Count);
        WorkerItems.Add(new WorkerItem(){ Worker = worker, WorkerTimeOfArrival = DateTime.Now});
	}

    public void RemoveWorker(Worker worker)
    {
        WorkerItem workerItem = WorkerItems.Find(n => n.Worker == worker);
        if(worker == null){
            Debug.LogError("worker can't be removed, isn't in WorkerItems!");
            return;
        }
        
        WorkerItems.Remove(workerItem);
    }

}
