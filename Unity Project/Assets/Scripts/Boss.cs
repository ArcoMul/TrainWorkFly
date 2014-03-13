using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boss : MonoBehaviour
{
	private static Boss _Instance = null;
	public static Boss Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = FindObjectOfType<Boss>();
			return _Instance;
		}
	}

    public Vector3 MoveToPosition;
    public bool Walking = false;

	public Transform[] IdlePositions;

    public List<Worker> Workers;
	
	void Update()
    {

		if (!Walking) return;

        transform.position += transform.right * Input.GetAxis("Horizontal") * 3 * Time.deltaTime;
        transform.position += transform.up * Input.GetAxis("Vertical") * 3 * Time.deltaTime;
    }

	public void WalkToCurrentMousePosition ()
	{
    	MoveToPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		Walking = true;
	}

	public Vector3 GetWorkerIdlePlace (Worker w)
	{
		int i = 0;
		foreach (Worker worker in Workers) {
			if (worker == w) return IdlePositions[i].position;
			i++;
		}
		return IdlePositions[Workers.Count].position;
	}

	public void AddIdleWorker (Worker w)
	{
		Workers.Add (w);
	}

	public void RemoveIdleWorker (Worker w)
	{
		Workers.Remove (w);
	}

	public void MakeWorkerJumpToIdle (Worker w)
	{
		w.transform.position = GetWorkerIdlePlace(w);
	}
}
