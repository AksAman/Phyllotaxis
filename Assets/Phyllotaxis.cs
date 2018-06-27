using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phyllotaxis : MonoBehaviour {

	public GameObject dot;
	public float angle;
	public float c;
	public int n;

	public Camera cam;
	public bool moveIn3D;
	Vector3 camCurrentPos;
	float offset = 1;

	void Start()
	{
		n = 0;
	}
	void Update()
	{
		GameObject dotGO = Instantiate (dot, this.transform) as GameObject;
		dotGO.transform.position = CalculatePosition (angle, n, c);
		n += 1;

		if(moveIn3D)
		{
			camCurrentPos = cam.transform.position;
			cam.transform.position = new Vector3 (camCurrentPos.x, camCurrentPos.y, camCurrentPos.z + offset * Time.deltaTime);
		}
	}

	Vector3 CalculatePosition(float _angle, int _number, float _scale)
	{
		float _radius = _scale * Mathf.Sqrt (_number);
		float _angleInRadians = _number * Mathf.Deg2Rad * _angle;
		float _x = _radius * Mathf.Cos (_angleInRadians);
		float _y = _radius * Mathf.Sin (_angleInRadians);
		float _z = moveIn3D ? _number * 0.1f : 0;
		return new Vector3 (_x,_y,_number * 0.1f);

	}

}

