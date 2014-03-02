using UnityEngine;
using System.Collections;

public class Deselecter : MonoBehaviour
{
	void OnMouseDown ()
	{
		Selecter.Instance.Deselect();
	}
}
