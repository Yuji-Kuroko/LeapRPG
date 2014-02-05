using UnityEngine;
using System.Collections;

public class ObjectWood : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision sender)
	{
		if (sender.gameObject.tag != "Magic")
			return;

		MagicBall magic = sender.gameObject.GetComponent<MagicBall>();
		if (magic.magicType == MagicBall.MagicType.Fire)
		{
			Destroy(gameObject);

		}

	}

}
