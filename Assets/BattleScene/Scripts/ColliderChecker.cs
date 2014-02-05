using UnityEngine;
using System.Collections;

public class ColliderChecker : MonoBehaviour {

	public bool isTriggering {get; private set;}
	public bool isCollisioning {get; private set;}
	public string collisionTag {get; private set;}

	[SerializeField]
	int triggerCount   = 0;
	[SerializeField]
	int collisionCount = 0;
	public int wallCount;
	int ctWallCount = 0;

	int okCount = 10;

	/// <summary>
	///  if Trigger or Collision are true, this is true. Other is false.
	/// </summary>
	/// <value><c>true</c> if is both collidering; otherwise, <c>false</c>.</value>
	public bool isBothCollidering
	{
		get
		{
			return (isTriggering || isCollisioning);
		}
	}


	// Use this for initialization
	void Start () {
		isTriggering = false;
		isCollisioning = false;
		collisionTag = "";
		wallCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (ctWallCount == 0)
			okCount++;
		else
			okCount = 0;
		if (okCount > 10)
			wallCount = 0;
		else
			wallCount = 1;
		ctWallCount = 0;

		if (triggerCount == 0)
			isTriggering = false;
		else
			isTriggering = true;

		if (collisionCount == 0)
			isCollisioning = false;
		else
			isCollisioning = true;
	}

	void OnTriggerEnter (Collider sender)
	{
		/*
		triggerCount++;
		collisionTag = sender.tag;
		if (sender.tag == "Wall")
			wallCount++;
		*/
	}

	void OnTriggerStay (Collider sender)
	{
		//triggerCount++;
		if (sender.tag == "Wall")
			ctWallCount++;
	}

	void OnTriggerExit (Collider sender)
	{
		/*
		triggerCount--;

		if (sender.tag == "Wall")
			wallCount--;
		*/
	}


	void OnCollisionEnter (Collision sender)
	{
		collisionTag = sender.gameObject.tag;
		if (sender.gameObject.tag == "Wall")
			wallCount++;
	}

	void OnCollisionExit (Collision sender)
	{
		if (sender.gameObject.tag == "Wall")
			wallCount--;
	}




}
