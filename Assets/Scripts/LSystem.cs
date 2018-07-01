using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour {
	TrailRenderer trailRend;
	Material trailMat;
	public Color trailColor;
	public Color SetColor;
	public Dictionary<char, string> rulesDict = new Dictionary<char, string>();
	private string axiom = "F";
	private string sent;

	private float angle = 25;
	public int generations;
	public float distance;

	private bool isGenerating;

	private Stack<TransformInfo> tiStack = new Stack<TransformInfo> ();

//	'Plant C':{
//		'axiom':'F',
//		'rules':{'F':'FF-[-F+F+F]+[+F-F-F]'},
//		'angle':22.5

	void Start()
	{
		trailRend = GetComponent<TrailRenderer> ();
		trailMat = trailRend.material;
//		trailColor= trailMat.color;
		rulesDict.Add ('F', "FF-[-F+F+F]+[+F-F-F]");
//		rulesDict.Add ('X', "F+[[X]-X]-F[-FX]+X");
//		rulesDict.Add('F',"FF");
		sent = axiom;
		StartCoroutine (GenerateLSystem ());
		Debug.Log (sent);
//		MoveTrails (sent);


	}

	void Update()
	{
//		trailColor = trailMat.GetColor("_TintColor");
//		trailMat.SetColor("_TintColor", SetColor);

	}

	IEnumerator GenerateLSystem()
	{
		int count = 0;
		while(count < generations)
		{
			if(isGenerating==false)
			{
				isGenerating = true;
				StartCoroutine (Generate ());
			}
			else if (isGenerating == true)
			{
				yield return new WaitForSeconds (0.1f);
			}
			
		}
	}

	IEnumerator Generate()
	{
		string tmpString = "";

		char[] sentChars = sent.ToCharArray ();

		foreach (char c in sentChars) 
		{
			if (rulesDict.ContainsKey(c)) 
			{
				tmpString += rulesDict[c];
			} 
			else 
			{
				tmpString += c.ToString();
			}
		}
		sent = tmpString;
		tmpString = "";

		foreach(char c in sent.ToCharArray())
		{
			if(c=='F')
			{
				//Move Forward
				Vector3 currentPos = transform.position;
//				Vector3 endPos = Vector3.Lerp (currentPos, transform.position, 0.05f); //
				transform.Translate(Vector3.forward * distance);
				Debug.DrawLine (currentPos, transform.position, Color.white, 100000f);
				Debug.Log ("Forward ");
				yield return null;

			}
			if(c=='+')
			{
				//Turn Left by angle
				transform.Rotate (Vector3.up * angle);
				Debug.Log ("Left ");
			}
			if(c=='-')
			{
				//Turn Right by angle
				transform.Rotate (Vector3.up * angle);
				Debug.Log ("Right ");
			}
			if(c=='[')
			{
				//Save Current position
				TransformInfo ti = new TransformInfo ();
				ti.position = transform.position;
				ti.rotation = transform.rotation;
				tiStack.Push (ti);
				Debug.Log ("Push ");
			}
			if(c==']')
			{
				//Get Back to the last saved position
				TransformInfo ti = tiStack.Pop ();
				transform.position = ti.position;
				transform.rotation = ti.rotation;
				Debug.Log ("Pop ");
			}
		}
		isGenerating = false;
	}

	void MoveTrails(string currentString)
	{
//		foreach(char c in currentString.ToCharArray())
//		{
//			if(c=='F')
//			{
//				//Move Forward
//				Vector3 currentPos = transform.position;
//				transform.Translate (Vector3.forward * distance);
//				Debug.DrawLine (currentPos, transform.position, Color.white, 100000f);
//				Debug.Log ("Forward ");
//
//			}
//			if(c=='+')
//			{
//				//Turn Left by angle
//				transform.Rotate (Vector3.up * angle);
//				Debug.Log ("Left ");
//			}
//			if(c=='-')
//			{
//				//Turn Right by angle
//				transform.Rotate (Vector3.up * angle);
//				Debug.Log ("Right ");
//			}
//			if(c=='[')
//			{
//				//Save Current position
//				TransformInfo ti = new TransformInfo ();
//				ti.position = transform.position;
//				ti.rotation = transform.rotation;
//				tiStack.Push (ti);
//				Debug.Log ("Push ");
//			}
//			if(c==']')
//			{
//				//Get Back to the last saved position
//				TransformInfo ti = tiStack.Pop ();
//				transform.position = ti.position;
//				transform.rotation = ti.rotation;
//				Debug.Log ("Pop ");
//			}
//		}
	}


}
