using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour {
	public GameObject Building;
	public List<TaskBuilding> Tasks;

	// Use this for initialization
	void Start () {
		Tasks = new List<TaskBuilding> ();
		CreateBuilding ();
	}

	public void CreateBuilding()
	{
		GameObject Obj = (GameObject) Instantiate (Building, new Vector3(transform.position.x - (Tasks.Count), transform.position.y, transform.position.z), new Quaternion(0, 0, 0, 0)); 
		Tasks.Add(Obj.GetComponent<TaskBuilding>());
	}	
}
