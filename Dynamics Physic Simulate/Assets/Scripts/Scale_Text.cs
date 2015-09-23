using UnityEngine;
using System.Collections;

public class Scale_Text : MonoBehaviour {

	public GUIText text;
	public int scaleValue;

	void Start () {
		text.fontSize = Mathf.Min (Screen.width, Screen.height) / scaleValue;
	}

	void Update () {
	
	}
}
