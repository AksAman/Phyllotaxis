using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePosition : MonoBehaviour {

	public Transform targetGO;
	void Update () {
		transform.position = new Vector3 (0, 0, targetGO.position.z);
	}
}
