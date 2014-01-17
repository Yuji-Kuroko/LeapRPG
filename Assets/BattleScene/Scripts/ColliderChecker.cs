using UnityEngine;
using System.Collections;

public class ColliderChecker : MonoBehaviour {

	public bool isTriggering {get; private set;}
	public bool isCollisioning {get; private set;}
	public string collisionTag {get; private set;}

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider sender)
	{
		isTriggering = true;
		try {
			collisionTag = sender.tag;
		}
		catch(UnityException e) {
			collisionTag = "";
		}

	}

	void OnTriggerExit (Collider sender)
	{
		isTriggering = false;
		collisionTag = "";
	}


	void OnCollisionEnter (Collision sender)
	{
		isCollisioning = true;
		try {
			collisionTag = sender.collider.tag;
		}
		catch (UnityException e) {
			collisionTag = "";
		}
	}

	void OnCollisionExit (Collision sender)
	{
		isCollisioning = false;
		collisionTag = "";
	}




}
