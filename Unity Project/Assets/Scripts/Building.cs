using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	void OnMouseDown ()
	{
		Worker worker = Selecter.Instance.Active.GetComponent<Worker>();
		if (worker == null)
		{
			Selecter.Instance.Select (GetComponent<Selectable>());
			return;
		}

		worker.SetGoal(this);
	}

	public Vector3 GetRestPosition ()
	{
		return transform.position - new Vector3 (1.2f, 0, 0);
	}
}
