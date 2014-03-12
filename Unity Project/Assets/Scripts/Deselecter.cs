using UnityEngine;
using System.Collections;

public class Deselecter : MonoBehaviour
{
	void OnMouseDown ()
	{
		Debug.Log("Deselect");
		Selecter.Instance.Deselect();
		Boss.Instance.WalkToCurrentMousePosition ();
	}
}
