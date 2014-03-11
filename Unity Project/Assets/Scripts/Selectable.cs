using UnityEngine;
using System.Collections;

/**
 * Makes an object selectable
 */
public class Selectable : MonoBehaviour
{
    public static bool IsSelectingInThisFrame = false;
	/**
	 * The sprites to switch between
	 */
	public Sprite SelectSprite;
	private Sprite DefaultSprite;

	/**
	 * If this class handles the onmousedown
	 */
	public bool SelectOnClick = true;

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
        IsSelectingInThisFrame = true;
		if (SelectOnClick) {
			Selecter.Instance.Select (this);
		}
	}

    void LateUpdate()
    {
        IsSelectingInThisFrame = false;
    }
	
}
