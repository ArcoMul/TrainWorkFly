using UnityEngine;
using System.Collections;

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
}
