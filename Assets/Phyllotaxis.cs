using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phyllotaxis : MonoBehaviour {

	public GameObject dot;
	public float angle;
	public float c;
	public int n;

	void Start()
	{
		n = 0;
	}
	void Update()
	{
		
//		dotGO.transform.position.z = 0;
//		if(Input.GetKey(KeyCode.Space))
//		{
			GameObject dotGO = Instantiate (dot, this.transform) as GameObject;
			dotGO.transform.position = CalculatePosition (angle, n, c);
			n += 1;
//		}
	}

	Vector3 CalculatePosition(float _angle, int _number, float _scale)
	{
		float _radius = _scale * Mathf.Sqrt (_number);
		float _angleInRadians = _number * Mathf.Deg2Rad * _angle;
		float _x = _radius * Mathf.Cos (_angleInRadians);
		float _y = _radius * Mathf.Sin (_angleInRadians);

		return new Vector3 (_x,_y,_number*0.1f);

	}

}

