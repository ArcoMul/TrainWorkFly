using UnityEngine;
using System.Collections;

public class Selecter : MonoBehaviour
{
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

	public Selectable Active; 

	public void Select (Selectable s)
	{
		if (Active != null) {
			Active.Deselect();
		}
		Active = s;
		Active.Select();
	}

	public void Deselect ()
	{
		Active.Deselect();
		Active = null;
	}

}
