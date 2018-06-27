using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhylloTrail : MonoBehaviour {

	private TrailRenderer trailRenderer;
	public float angle, scale;
	public int startNumber;
	int currentnumber;
	public int stepSize;
	private Vector2 phylloPosition;

	// Iterations
	public int maxIterations;
	private int currentIteration;
	public float percentageComplete;

	public bool useLerping;
	private bool isLerping;
	public float lerpInterval;
	private Vector3 startPos, endPos;
	float timeStartedlerping, timeSinceLerping;

	public bool moveIn3D;

	void Awake()
	{
		trailRenderer = GetComponent<TrailRenderer> ();
		currentnumber = startNumber;
		transform.localPosition = CalculatePosition (angle, currentnumber, scale);
		if (useLerping)
		{
			StartLerping ();
		}
	}

	void FixedUpdate()
	{
		if (useLerping) 
		{
			if(isLerping)
			{
				timeSinceLerping = Time.time - timeStartedlerping;
				percentageComplete = timeSinceLerping / lerpInterval;
				transform.localPosition = Vector3.Lerp (startPos, endPos, percentageComplete);

				if(percentageComplete >=0.97f)
				{
					transform.localPosition = endPos;
					isLerping = false;
					currentnumber += stepSize;
					currentIteration++;
					if(currentIteration <= maxIterations)
					{
						StartLerping();
					}
					else
					{
						isLerping = false;
					}
				}
			}
		} 

		else
		{
			transform.localPosition = CalculatePosition (angle, currentnumber, scale);
			currentnumber += stepSize;
		}
	}

	Vector2 CalculatePosition(float _angle, int _number, float _scale)
	{
		float _radius = _scale * Mathf.Sqrt (_number);
		float _angleInRadians = _number * Mathf.Deg2Rad * _angle;
		float _x = _radius * Mathf.Cos (_angleInRadians);
		float _y = _radius * Mathf.Sin (_angleInRadians);
		return new Vector2 (_x, _y);
	}
		
	void StartLerping()
	{
		isLerping = true;
		startPos = transform.position;
		phylloPosition = CalculatePosition (angle, currentnumber, scale);
		endPos = new Vector3 (phylloPosition.x, phylloPosition.y, currentnumber*0.1f);
		timeStartedlerping = Time.time;
	}
}
