using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchToScale : MonoBehaviour
{
	private float initialDistance;

	private Vector3 initialScale;

	// Update is called once per frame
	void Update()
	{
		if(Input.touchCount == 2)
		{
			var touchZero = Input.GetTouch(0);
			var touchOne = Input.GetTouch(1);
   
			if(touchZero.phase == TouchPhase.Ended || touchZero.phase == TouchPhase.Canceled ||
				touchOne.phase == TouchPhase.Ended || touchOne.phase == TouchPhase.Canceled)
			{
				return;
			}

			if(touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)
			{
				initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
				initialScale = transform.localScale;
				//Debug.Log("Initial Disatance: " + initialDistance + "GameObject Name: "
				// + transform.gameObject.name);
			}
			else
			{
				var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

				if (Mathf.Approximately(initialDistance, 0))
				{
					return; // do nothing if it can be ignored where inital distance is very close to zero
				}

				var factor = currentDistance / initialDistance;
				transform.localScale = initialScale * factor; // scale multiplied by the factor we calculated
			}
		}
	}
}
