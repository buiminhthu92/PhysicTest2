using UnityEngine;
using System.Collections;

public class testChild : MonoBehaviour {

	public bool isChild = false;
	public Transform child;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isChild) {
			child.parent = transform;
		}
		else {
			child.parent = null;
		}
	}
}
