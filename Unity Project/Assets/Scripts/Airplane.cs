using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Airplane : MonoBehaviour {

    public static Airplane CurrentAirplane;

    public GameObject Building;
    public List<TaskBuilding> Tasks;

    public int TimeSinceSpawn;
    public int TimeToFinishPlane;

    public bool Incoming = true;
    public bool GoingOut = false;

    void Awake(){
        CurrentAirplane = this;
        Incoming = true;
    }

    void Start()
    {
        CreateTasks();
        StartCoroutine(Check());
    }

    void Update()
    {
        if (Incoming || GoingOut)
        {
            // Get the direction from the worker to the building and reset the z axis
            Vector3 Direction = Vector3.zero;
            if (Incoming)
            {
                Direction = GameObject.Find("IncomingPoint").transform.position - transform.position;
            }
            else if (GoingOut)
            {
                Direction = GameObject.Find("GoingOutPoint").transform.position - transform.position;
            }

            Direction = new Vector3(Direction.x, Direction.y, 0);

            // Calculate the movement for this frame and add this to the position
            Vector3 Movement = Direction.normalized * Time.deltaTime * 5;
            transform.position += Movement;

            if (Mathf.Abs(Movement.x) > Mathf.Abs(Direction.x) - 0.01f && Mathf.Abs(Movement.y) > Mathf.Abs(Direction.y) - 0.01f)
            {
                if (Incoming)
                {
                    Incoming = false;
                }
                if (GoingOut)
                {
                    SucceededPlane();
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void CreateTasks()
    {
        Tasks = new List<TaskBuilding>();
        TaskBuilding[] TasksArray = this.GetComponentsInChildren<TaskBuilding>();
        foreach (TaskBuilding task in TasksArray)
        {
            Tasks.Add(task);
        }
    }

    public IEnumerator Check()
    {
        Debug.Log("Creating building1");
        while (true)
        {
            GameObject.Find("TimeLeft").GetComponent<TextMesh>().text = "Time left: " + (TimeToFinishPlane - TimeSinceSpawn);
            int completed = 0;
            completed = Tasks.FindAll(n => n.IsCompleted).Count;
            if (completed == Tasks.Count)
            {
                Debug.Log("Completed " + Tasks.Count);
                GoingOut = true;
                yield break;
            }
            TimeSinceSpawn += 1;
            if (TimeSinceSpawn > TimeToFinishPlane)
            {
                FailedPlane();
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void SucceededPlane()
    {
        Debug.Log("SucceededPlane");
        // TODO: Let next plane in
        AirplaneManager.Instance.SpawnNextAirplane();
    }

    public void FailedPlane()
    {
        // Game over Scene?
        Application.LoadLevel(1);
    }
}
