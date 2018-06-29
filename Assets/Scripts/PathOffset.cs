using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOffset : MonoBehaviour {

	public float amplitude;
	private float offset;

	void Update()
	{
		offset = amplitude * Mathf.Sin (Time.time);
		transform.position = new Vector3 (transform.position.x+offset, transform.position.y+offset, transform.position.z);
	}
}
