using UnityEngine;
using System.Collections;

public class testMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown ()
	{
		rigidbody.AddForce(-transform.forward * 500f);
		rigidbody.useGravity = true;
	}
}
