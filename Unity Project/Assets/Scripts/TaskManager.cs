using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
	public GameObject Building;
	public List<TaskBuilding> Tasks;
	public int TimeFailTask;
	public int TimeFinishTask;
	public int TimeMultiplier;

	private int BuildingCount = 0;

	void Start ()
	{
		Tasks = new List<TaskBuilding> ();
        StartCoroutine(CreateBuildings());
	}

	 
	public IEnumerator CreateBuildings()
	{
        while (true)
        {
            Debug.Log("Creating building");
            BuildingCount++;
            GameObject Obj = (GameObject)Instantiate(Building, new Vector3(transform.position.x + 5, transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 0));
            TaskBuilding TaskBuilding = Obj.GetComponent<TaskBuilding>();
            TaskBuilding.TimeToFailTask = TimeFailTask - (BuildingCount * TimeMultiplier);
            TaskBuilding.TimeToFinishTask = TimeFinishTask + (BuildingCount * TimeMultiplier);
            TaskBuilding.MoveToPosition = new Vector3(transform.position.x + (Tasks.Count * 2), transform.position.y, transform.position.z);
            TaskBuilding.State = TaskBuilding.States.Moving;
            TaskBuilding.Manager = this;
            Tasks.Add(TaskBuilding);
            yield return new WaitForSeconds(45 - (BuildingCount * 5));
        }
	}

	public void UpdateOnFinishTask()
	{
        Debug.Log("TASKS: " + Tasks.Count);
		Tasks.RemoveAt (0);
		int index = 0;

		foreach (TaskBuilding b in Tasks)
		{
			b.State = TaskBuilding.States.Moving;
			b.MoveToPosition = new Vector3(transform.position.x + (index * 2), transform.position.y, transform.position.z);
			index++;
		}
	}

}
