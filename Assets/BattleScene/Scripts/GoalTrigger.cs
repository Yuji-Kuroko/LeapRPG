using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {

	public GUITexture GoalLogo;

	bool isGoaled = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider sender)
	{
		if (isGoaled)
			return;
		isGoaled = true;

		StartCoroutine("GoalEffect");



	}

	IEnumerator GoalEffect ()
	{
		GoalLogo.enabled = true;
		audio.Play();
		GameObject.Find("BGM").audio.Stop();

		yield return new WaitForSeconds(5.0f);

		//	to title scene
		Application.LoadLevel(0);

	}


}
