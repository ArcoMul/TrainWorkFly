using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour
{
	public Sprite SelectSprite;
	private Sprite DefaultSprite;

	void Start ()
	{
		DefaultSprite = GetComponent<SpriteRenderer>().sprite;
	}

	public void Select ()
	{
		GetComponent<SpriteRenderer>().sprite = SelectSprite;
	}

	public void Deselect ()
	{
		GetComponent<SpriteRenderer>().sprite = DefaultSprite;
	}

	void OnMouseDown ()
	{
		Selecter.Instance.Select (this);
	}
}
