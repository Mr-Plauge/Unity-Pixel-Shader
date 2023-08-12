using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {
	public Blackout_Controller blackout;

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
            blackout.StartCoroutine(blackout.Fade(true, 1));
		}
	}
}
