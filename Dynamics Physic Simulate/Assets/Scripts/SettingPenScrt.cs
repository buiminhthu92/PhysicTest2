using UnityEngine;
using System.Collections;

public class SettingPenScrt : MonoBehaviour {

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
	private Vector2 uvVl;

	//test value
	private float k1, k2, k3, k4, m1, m2, m3, m4, oomega, ttheta;


	void Start () {
	
	}

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
		ttheta = TransDegtoRad(degreeValue);
		Debug.Log(theta);
		float b = TransRadtoDeg(theta);
		Debug.Log(b);

		if(Play_Pause_Scrt.isPlay) {
			Ball.parent = transform;
			t += h;
			uvVl = RK4(omega, theta, t);
			omega = uvVl.x;
			theta = uvVl.y;

			penLength.x = 0;
			penLength.y = 0;
			penLength.z = TransRadtoDeg(theta);
			Debug.Log("theta:" + penLength.z + "omega:" + omega + ", time:" + t);
			transform.eulerAngles = penLength;
		}

		Ball.parent = null;
	}

	#region
	//test simple funct
	public float f1(float omega, float theta, float t) {
		float a = -Mathf.Sin(theta);
		return a;
	}
	public float f2(float omega, float theta, float t) {
		float a = omega;
		return a;
	}
	#endregion

	//Set Ball usually follow the point ball
	void BallFollowPoint(Transform moveObj, Transform followObj) {
		Vector3 movePos = moveObj.position;
		followObj.position = movePos;
	}

	//Runge-Kutta method
	public Vector2 RK4(float u, float v, float t) {
		float k1, k2, k3, k4, l1, l2, l3, l4;
		
		k1 = dt * udot(u, v, t);
		l1 = dt * vdot(u, v, t);
		
		k2 = dt * udot(u + k1 / 2, v + l1 / 2, t + dt * 0.5f);
		l2 = dt * vdot(u + k1 / 2, v + l1 / 2, t + dt * 0.5f);
		
		k3 = dt * udot(u + k2 / 2, v + l2 / 2, t + dt * 0.5f);
		l3 = dt * vdot(u + k2 / 2, v + l2 / 2, t + dt * 0.5f);
		
		k4 = dt * udot(u + k3, v + l3, t + dt);
		l4 = dt * vdot(u + k3, v + l3, t + dt);
		
		u = u + (k1 + 2 * (k2 + k3) + k4) / 6;
		v = v + (l1 + 2 * (l2 + l3) + l4) / 6;

		#region
//		k1 = udot(u, v, t);
//		l1 = vdot(u, v, t);
//		
//		k2 = udot(u + (k1 * dt) / 2, v + (l1 * dt) / 2, t + (0.5f * dt));
//		l2 = vdot(u + (k1 * dt) / 2, v + (l1 * dt) / 2, t + (0.5f * dt));
//		
//		k3 = udot(u + (k2 * dt) / 2, v + (l2 * dt) / 2, t + (0.5f * dt));
//		l3 = vdot(u + (k2 * dt) / 2, v + (l2 * dt) / 2, t + (0.5f * dt));
//		
//		k4 = udot(u + k3 * dt, v + l3 * dt, t + dt);
//		l4 = vdot(u + k3 * dt, v + l3 * dt, t + dt);
//		
//		u = u + (k1 + 2 * (k2 + k3) + k4) * dt / 6;
//		v = v + (l1 + 2 * (l2 + l3) + l4) * dt / 6;
		#endregion
		
		return new Vector2(u, v);
	}
	
	public float udot(float x, float y, float t) { /**correspomding omega, theta, time**/
		//float a = (-OmegaSQ * OmegaSQ * Mathf.Sin(y) - q * x + fd * Mathf.Sin(t)) * dt;
		float a = -Mathf.Sin(y); //test simple line
		return a;
	}
	
	public float vdot(float x, float y, float t) { /**correspomding omega, theta, time**/
		//return (x * dt);
		return x; //test simple line
	}

	public float TransDegtoRad(float degree) {
		float rad = degree * (Mathf.PI / 180);
		return rad;
	}

	public float TransRadtoDeg(float radian) {
		float deg = radian * (180 / Mathf.PI);
		return deg;
	}
}
