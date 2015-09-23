using UnityEngine;
using System.Collections;

public class Play_Pause_Scrt : MonoBehaviour {


	public static bool isPlay = false;
	public Texture playTexture;
	public Texture pauseTexture;
	public GameObject slideGroupObj;
	public GUITexture playBtn;



	void Start () {
		playBtn.texture = playTexture;
	}

	void Update () {

		//Detect touch button
		if(Input.touches.Length <= 0) {
			//no touches
		}
		else {
			for(int i = 0; i < Input.touchCount; i++) {
				if(playBtn.guiTexture.HitTest(Input.GetTouch(i).position)) {
					if(Input.GetTouch(i).phase == TouchPhase.Ended) {
						isPlay = !isPlay;
					}
				}
			}
		}

		if(isPlay) {
			playBtn.texture = pauseTexture;
			slideGroupObj.SetActive(false);
		}
		if(!isPlay) {
			playBtn.texture = playTexture;
			slideGroupObj.SetActive(true);
		}
	}

//	void OnMouseDown() {
//		isPlay = !isPlay;
//		Debug.Log("OK");
//
//		if(isPlay) {
//			playBtn.texture = pauseTexture;
//			slideGroupObj.SetActive(false);
//		}
//		if(!isPlay) {
//			playBtn.texture = playTexture;
//			slideGroupObj.SetActive(true);
//		}
//	}
}
