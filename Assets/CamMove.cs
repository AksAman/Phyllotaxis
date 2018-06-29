﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {

	public float speed;
	public float angle;
	// Use this for initialization
	public Visualizer visual;
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime * visual.audioBandBuffer[0]*100);
		transform.RotateAround (Vector3.forward, angle*Mathf.Deg2Rad * visual.audioBandBuffer[0]*10);
	}
}