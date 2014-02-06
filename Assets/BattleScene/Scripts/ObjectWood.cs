using UnityEngine;
using System.Collections;

public class ObjectWood : MonoBehaviour {

	public GameObject woodDust;
	public float Volume = 1.0f;


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

			GameObject dust = (GameObject)Instantiate(woodDust, transform.position, Quaternion.identity);
			dust.audio.volume = Volume;
			Destroy(gameObject);

		}

	}

}
