using UnityEngine;
using System.Collections;
using Leap;

public class MoverPlayerController : MonoBehaviour {

	public Controller leapController = new Controller();

	public ColliderChecker forwardChecker;

	public static float MOVE_SPEED = 6.0f / 45.0f;

	bool isAct = false;

	public AudioClip SeFootstep;


	enum ActType {
		MoveForward,
		MoveBack,
		TurnRight,
		TurnLeft,
		None,
	}

	ActType nowAct = ActType.None;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//	leap see one hand only.
		Frame frame = leapController.Frame();
		if (frame.Hands.Count != 1)
			return;
		if (isAct)
			return;

		Hand hand = frame.Hands[0];
		if (hand.PalmPosition.z < -10)
		{
			//	if exist wall for forward, can't forward.
			if (CanForward())
			{
				nowAct = ActType.MoveForward;
				StartCoroutine("MoveAction");
			}
		}
		else if (hand.PalmPosition.x > 60)
		{
			nowAct = ActType.TurnRight;
			StartCoroutine("MoveAction");
		}
		else if (hand.PalmPosition.x < -60)
		{
			nowAct = ActType.TurnLeft;
			StartCoroutine("MoveAction");
		}
	}

	void OnTriggerEnter (Collider sender)
	{
		Debug.Log("Trigger Enter");
	}

	void OnTriggerExit (Collider sender)
	{
		Debug.Log("Trigger Exit");
	}

	IEnumerator MoveAction()
	{
		 if (nowAct == ActType.MoveForward)
			PlayFootStep();


		isAct = true;
		for (int i = 0; i < 45; i++)
		{
			switch (nowAct)
			{
			case ActType.MoveForward:
				MoveForward();
				break;
			case ActType.MoveBack:
				MoveBack();
				break;
			case ActType.TurnRight:
				TurnRight();
				break;
			case ActType.TurnLeft:
				TurnLeft();
				break;

			}

			yield return null;
		}


		isAct = false;
	}


	void TurnRight() {
		transform.Rotate(new Vector3(0, 2.0f, 0));

	}

	void TurnLeft() {
		transform.Rotate(new Vector3(0, -2.0f, 0));

	}

	void MoveForward() {
		//Vector3 pos = transform.position;
		//transform.position = new Vector3(pos.x, pos.y, pos.z + MOVE_SPEED);
		transform.Translate(new Vector3(0, 0, MOVE_SPEED));

	}

	void MoveBack() {
		transform.Translate(new Vector3(0, 0, MOVE_SPEED));
	}



	bool CanForward() {
		if (forwardChecker.isBothCollidering && forwardChecker.collisionTag == "Wall")
			return false;
		return true;
	}

	void PlayFootStep() {
		StartCoroutine("_playFootStep");

	}

	IEnumerator _playFootStep() {
		audio.PlayOneShot(SeFootstep);
		yield return new WaitForSeconds(0.4f);
		audio.PlayOneShot(SeFootstep);
	}

}
