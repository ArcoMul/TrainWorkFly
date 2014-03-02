using UnityEngine;
using System.Collections;

/**
 * A building is a place a worker can walk to, so far
 */
public class Building : MonoBehaviour
{
	/**
	 * On mouse down check if a worker is selected, if so give this building
	 * as goal for the worker. If not select the building.
	 */
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

	/**
	 * Returns the position where a worker can 'rest'
	 */
	public Vector3 GetRestPosition ()
	{
		return transform.position - new Vector3 (1.2f, 0, 0);
	}
}
