using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour {
	TrailRenderer trailRend;

	public string axiom, ruleIn, ruleOut;
	string sent;

	public float angle;
	public int generations;
	public float distance;
	bool loop;

	char F = 'F';
	char dash = '-';
	char plus = '+';

//	'Plant C':{
//		'axiom':'F',
//		'rules':{'F':'FF-[-F+F+F]+[+F-F-F]'},
//		'angle':22.5

	void Update()
	{
//		if(Input.GetKeyDown(KeyCode.UpArrow))
//		{
//			Forward (gameObject);
//		}
//		else if(Input.GetKeyDown(KeyCode.LeftArrow))
//		{
//			Left (gameObject);
//		}
//		else if(Input.GetKeyDown(KeyCode.RightArrow))
//		{
//			Right (gameObject);
//		}
//		else if(Input.GetKeyDown(KeyCode.DownArrow))
//		{
//			Down (gameObject);
//		}

		foreach (char c in sent)
		{
			if(c=='F')
			{
				Forward ();
			}
			if(c=='-')
			{
				Right ();
			}
			if(c== '+')
			{
				Left ();
			}

		}
	}

	void Awake()
	{
		trailRend = GetComponent<TrailRenderer> ();
	}

	void Start()
	{
		sent = GenerateString (generations);
		print ("Sent " + sent+"\n");

	}

	string GenerateString(int gens)
	{
		string tmpSent = axiom;
		string tmpString = "";

		for (int i = 0; i < gens; i++) 
		{
			foreach (char c in tmpSent) 
			{
				if (string.Compare(c.ToString(), ruleIn) == 0) 
				{
					tmpString += ruleOut;
				} 
				else 
				{
					tmpString += c;
				}
			}
			tmpSent = tmpString;
			tmpString = "";
		}
		return tmpSent;
	}

	void Forward()
	{
		this.gameObject.transform.Translate (new Vector3 (0, 0, distance));
	}

	void Left()
	{
		this.gameObject.transform.Translate (new Vector3 (-distance, 0, 0));
	}

	void Right()
	{
		this.gameObject.transform.Translate (new Vector3 (distance, 0, 0));
	}

	void Down()
	{
		this.gameObject.transform.Translate (new Vector3 (0, 0, -distance));
	}

}
