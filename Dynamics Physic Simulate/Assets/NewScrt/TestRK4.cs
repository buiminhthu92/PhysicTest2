using UnityEngine;
using System.Collections;

public class TestRK4 : MonoBehaviour {

	public Transform ballPoint;
	public Transform Ball;
	
	//Slide object of setting pendulum
	public Slider lengthObj;
	public Slider degreeObj;
	public Slider forceObj;
	public Slider omegaObj;
	public Slider frictionObj;
	
	//Value setting of pendulum
	private float lengthValue;
	private float degreeValue;
	private float forceValue;
	private float omegaValue;
	private float frictionValue;
	
	private Vector3 ballPos; //Position of ball
	private Vector3 penDegree; //Degree of pendulum
	private Vector3 penLength; //Local scale of rope
	
	//Set function
	public float dt = 0.01f;
	public float h = 0.1f;
	public float G = 9.8f;
	private float fd = 0.0f; //The ampitude of of the driving force(FD) fd = FD/omegaD^2 // omegaD is omegaValue
	private float OmegaSQ = 0.0f; //OmegaSQ(Omega Square) = Omega^2 = (1/omegaD^2)*(G/l) // omegaD is omegaValue
	private float q = 0.0f; //Damping value discribe by q = (Q/omegaD) //(Q is frictionValue) //omegaD is omegaValue
	private float omega = 0.0f, theta = 0.0f, t = 0.0f;
	private Vector3 uvVl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Get all of pendulum setting value from slide objects 
		lengthValue = lengthObj.GetSliderPercent();
		degreeValue = degreeObj.GetSliderPercent();
		forceValue = forceObj.GetSliderPercent();
		omegaValue = omegaObj.GetSliderPercent();
		frictionValue = frictionObj.GetSliderPercent();
		
		//Set length of the rope equal length slide value
		penLength = transform.localScale;
		penLength.y = lengthValue;
		transform.localScale = penLength;
		
		//Set degree of pendulum equal degree slide value
		penDegree = transform.eulerAngles;
		penDegree.z = degreeValue;
		transform.eulerAngles = penDegree;
		
		BallFollowPoint(ballPoint, Ball);
		
		if(omegaValue == 0) {
			fd = 0;
			q = 0;
			OmegaSQ = 0;
		}
		if(omegaValue != 0) {
			//Callculate fd
			fd = forceValue / omegaValue;
			//Calculate q
			q = frictionValue / omegaValue; //Expect q = 0.75f
			//Calculate Omega
			OmegaSQ = (1/(omegaValue * omegaValue)) * (G / lengthValue); //Expect OmegaSQ = 1.5f
		}
		
		theta = TransDegtoRad(degreeValue);
		Debug.Log(theta);
		float b = TransRadtoDeg(theta);
		Debug.Log(b);
		
		if(Play_Pause_Scrt.isPlay) {
			Ball.parent = transform;
			uvVl = Test_RK4(t, theta, omega, h);
			omega = uvVl.x;
			theta = uvVl.y;
			t = uvVl.z;
			
			penLength.x = 0;
			penLength.y = 0;
			penLength.z = TransRadtoDeg(theta);
			Debug.Log("theta:" + penLength.z + "omega:" + omega + ", time: " + t);
			transform.eulerAngles = penLength;
		}
		
		Ball.parent = null;
	}

	public Vector3 Test_RK4(float tn, float xn, float yn, float h) {
		float k1, k2, k3, k4, l1, l2, l3, l4, k, l;
		Vector3 result;

		k1 = f (tn, xn, yn);
		l1 = g (tn, xn, yn);

		k2 = f (tn + 0.5f * h, xn + 0.5f * h * k1, yn + 0.5f * h * l1);
		l2 = g (tn + 0.5f * h, xn + 0.5f * h * k1, yn + 0.5f * h * l1);

		k3 = f (tn + 0.5f * h, xn + 0.5f * h * k2, yn + 0.5f * h * l2);
		l3 = g (tn + 0.5f * h, xn + 0.5f * h * k2, yn + 0.5f * h * l2);

		k4 = f (tn + h, xn + h * k3, yn + h * l3);
		l4 = g (tn + h, xn + h * k3, yn + h * l3);

		k = (1 / 6) * (k1 + 2 * (k2 + k3) + k4);
		l = (1 / 6) * (l1 + 2 * (l2 + l3) + l4);

		result.x = xn + h * k;
		result.y = yn + h * l;
		result.y = tn + h;

		return result;
	}

	public float f(float t, float x, float y) {
		float deriv = -x;
		return deriv;
	}

	public float g(float t, float x, float y) {
		return -Mathf.Sin(y);
	}

	public float TransDegtoRad(float degree) {
		float rad = degree * (Mathf.PI / 180);
		return rad;
	}
	
	public float TransRadtoDeg(float radian) {
		float deg = radian * (180 / Mathf.PI);
		return deg;
	}

	//Set Ball usually follow the point ball
	void BallFollowPoint(Transform moveObj, Transform followObj) {
		Vector3 movePos = moveObj.position;
		followObj.position = movePos;
	}

}
