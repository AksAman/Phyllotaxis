using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhylloTrail : MonoBehaviour {

	// Basic Variables
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

	// Lerping
	public bool useLerping;
	private bool isLerping;
	private Vector3 startPos, endPos;
	public AnimationCurve lerpSpeedCurve;
	public Vector2 lerpPosMinMaxSpeed;
	public float lerpPosTimer, lerpPosSpeed;
	public int lerpBand;

	// Audio
	public Visualizer visual;
	public Color trailColor;
	private Material trailMat;


	public bool moveIn3D;

	void Awake()
	{
		trailRenderer = GetComponent<TrailRenderer> ();
//		trailMat = new Material (trailRenderer.material);
//		trailMat.SetColor ("_TintColor", trailColor);
//		trailRenderer.material = trailMat;

		currentnumber = startNumber;
		transform.localPosition = CalculatePosition (angle, currentnumber, scale);
		if (useLerping)
		{
			isLerping = true;
			StartLerping ();
		}
	}

	void Update()
	{
		if(useLerping)
		{
			if(isLerping)
			{
				float audioValue = visual.audioBandBuffer [lerpBand] * 1000;
				audioValue = Mathf.Clamp01(audioValue);
				Debug.Log ("On Anim curve : " + lerpSpeedCurve.Evaluate (audioValue));
				lerpPosSpeed = Mathf.Lerp (lerpPosMinMaxSpeed.x, lerpPosMinMaxSpeed.y, lerpSpeedCurve.Evaluate (audioValue));
				lerpPosTimer += Time.deltaTime * lerpPosSpeed;

				transform.localPosition = Vector3.Lerp (startPos, endPos, Mathf.Clamp01(lerpPosTimer));
				Debug.Log (lerpPosTimer);
				if(lerpPosTimer >= 1)
				{
					lerpPosTimer -= 1;
					currentnumber += stepSize;
					currentIteration++;
					StartLerping ();
				}
			}
		}

		if(!useLerping)
		{
			transform.localPosition = CalculatePosition (angle, currentnumber, scale);
			currentnumber += stepSize;
			currentIteration++;
		}
	}

	/* void FixedUpdate()
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
	}*/

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
		startPos = transform.position;
		phylloPosition = CalculatePosition (angle, currentnumber, scale);
		endPos = new Vector3 (phylloPosition.x, phylloPosition.y, currentnumber);
	}
}
