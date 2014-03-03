using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour {
	public GameObject Building;
	public List<TaskBuilding> Tasks;

	private int BuildingCount = 0;

	// Use this for initialization
	void Start () {
		Tasks = new List<TaskBuilding> ();
		CreateBuilding ();
		CreateBuilding ();
		CreateBuilding ();
	}
	 
	public void CreateBuilding()
	{
		BuildingCount++;
		GameObject Obj = (GameObject) Instantiate (Building, new Vector3(transform.position.x + (Tasks.Count * 2), transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 0)); 
		TaskBuilding TaskBuilding = Obj.GetComponent<TaskBuilding> ();
		TaskBuilding.TimeToFailTask = 120 - (BuildingCount * 10);
		TaskBuilding.TimeToFinishTask = 30 + (BuildingCount * 10);
		Tasks.Add(TaskBuilding);
	}	
}
