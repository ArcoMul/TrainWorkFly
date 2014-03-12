using UnityEngine;
using System.Collections;

/**
 * Manages selecting of objects in the level
 */
public class Selecter : MonoBehaviour
{
	/**
	 * Returns instance of this object, for easy use in the scene, there is always only one selecter
	 */
	private static Selecter _Instance = null;
	public static Selecter Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = FindObjectOfType<Selecter>();
			return _Instance;
		}
	}

	/**
	 * The selectable which is currently selected
	 */
	public Selectable Active; 

	/**
	 * Selects a selectable
	 */
	public void Select (Selectable s)
	{
		if (Active != null) {
			Active.Deselect();
		}
        if (s == null)
        {
            Debug.LogError("Selectable is null, should happen");
            return;
        }
		Active = s;
		Active.Select();
	}

	/**
	 * Deselects the current active selectable
	 */
	public void Deselect ()
	{
		if (Active != null) {
			Active.Deselect();
		}
		Active = null;
	}

}
