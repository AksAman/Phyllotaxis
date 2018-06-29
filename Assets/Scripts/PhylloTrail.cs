using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhylloTrail : MonoBehaviour {

	// Basic Variables
	private TrailRenderer trailRenderer;

	[Tooltip("Angle and Scale")]
	public float angle, scale;
	public int startNumber;
	int currentnumber;

	public int stepSize;
	private Vector2 phylloPosition;
	public int zOffset;
	public float zSmallOffset;
	public float amplitude;
	private float offset;

	//Current Trail Position
	private Vector3 currentTrailPos;

	// Iterations
	public int maxIterations;
	private int currentIteration;

	[Header("Audio Visualizer")]
	public Visualizer visual;

	// Lerping
	[Header("Lerping Options")]
	public bool useLerping;
	private bool isLerping;
	private Vector3 startPos, endPos;
	public AnimationCurve lerpSpeedCurve;
	public Vector2 lerpPosMinMaxSpeed;
	private float lerpPosTimer, lerpPosSpeed;
	public int lerpBand;

	// Audio
	public Color trailColor;
	private Material trailMat;

	// Repeat?
	public bool repeat, invert;
	private bool forward;

	//Scaling on audio
	[Header("Scaling on audio Options")]
	public bool useScaling;
	public bool useScaleAnimCurve;
	public Vector2 scaleMinMax;
	public int scaleBand;
	public AnimationCurve scaleAnimCurve;
	public float scaleTimer;
	public float currentScale, scaleSpeed;

	[Header("3D Trails?")]
	public bool moveIn3D;

	void Awake()
	{
		currentScale = scale;
		zOffset = 0;
		trailRenderer = GetComponent<TrailRenderer> ();
//		trailMat = new Material (trailRenderer.sharedMaterial);
//		trailMat.SetColor ("_TintColor", trailColor);
//		trailRenderer.sharedMaterial = trailMat;

		forward = true;

		currentnumber = startNumber;
		transform.localPosition = CalculatePosition (angle, currentnumber, currentScale);
		if (useLerping)
		{
			isLerping = true;
			StartLerping ();
		}
	}

	void Update()
	{
		if(useScaling)
		{
			if(useScaleAnimCurve)
			{
				scaleTimer = visual.audioBandBuffer [scaleBand] * Time.deltaTime * scaleSpeed;
				currentScale = Mathf.Lerp (scaleMinMax.x, scaleMinMax.y, scaleAnimCurve.Evaluate(scaleTimer));

				if(scaleTimer > 1)
				{
					scaleTimer -= 1;
				}
			}
			else if (!useScaleAnimCurve) 
			{
				currentScale = Mathf.Lerp (scaleMinMax.x, scaleMinMax.y, visual.audioBandBuffer [scaleBand] * scaleSpeed);
			}
		}

		if(useLerping)
		{
			if(isLerping)
			{
				float audioValue = visual.audioBandBuffer [lerpBand] * 1000;
				audioValue = Mathf.Clamp01(audioValue);
				lerpPosSpeed = Mathf.Lerp (lerpPosMinMaxSpeed.x, lerpPosMinMaxSpeed.y, lerpSpeedCurve.Evaluate (audioValue));
				lerpPosTimer += Time.deltaTime * lerpPosSpeed;

				currentTrailPos = Vector3.Lerp (startPos, endPos, Mathf.Clamp01(lerpPosTimer));
				transform.localPosition = currentTrailPos;

				if(lerpPosTimer >= 1)
				{
					lerpPosTimer -= 1;
					if(forward)
					{
						currentnumber += stepSize;
						currentIteration++;
						StartLerping ();
					}
					else if(!forward)
					{
						currentnumber -= stepSize;
						currentIteration--;

					}

					if(currentIteration > 0 && currentIteration < maxIterations)
					{
						StartLerping ();
					}

					else
					{
						if(repeat)
						{
							if(invert)
							{
								forward = !forward;
								StartLerping ();
							}

							else
							{
								currentIteration = 0;
								currentnumber = startNumber;
								StartLerping ();
							}
						}

						else if(!repeat)
						{
							isLerping = false;
						}
					}
				}
			}
		}

		if(!useLerping)
		{
			transform.localPosition = CalculatePosition (angle, currentnumber, currentScale);
			currentnumber += stepSize;
			currentIteration++;
		}
	}

	Vector2 CalculatePosition(float _angle, int _number, float _scale)
	{
		float _radius = _scale * Mathf.Sqrt (_number);
		float _angleInRadians = _number * Mathf.Deg2Rad * _angle;
		float _x = _radius * Mathf.Cos (_angleInRadians);
		float _y = _radius * Mathf.Sin (_angleInRadians);
		offset = amplitude * Mathf.Sin (Time.time);
//		_x += offset;
//		_y += offset;
		return new Vector2 (_x, _y);
	}

	void StartLerping()
	{
		zOffset += 1;
		startPos = transform.position;
		phylloPosition = CalculatePosition (angle, currentnumber, currentScale);
		float _z = moveIn3D ? zOffset * zSmallOffset: 0;
		endPos = new Vector3 (phylloPosition.x, phylloPosition.y, _z);
	}

//	public Vector3 ReturnCurrentCenter()
//	{
//		
//	}
}
