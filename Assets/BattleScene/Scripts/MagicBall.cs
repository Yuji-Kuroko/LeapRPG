using UnityEngine;
using System.Collections;

public class MagicBall : MonoBehaviour {

	public enum ShooterType
	{
		Player, Enemy, Trap
	}

	public ShooterType shooter { get; private set; }
	public int magicLevel { get; private set; }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0, 0, 0.2f));
	}

	void OnCollisionEnter (Collision sender)
	{
		//	Collision with wall



		Destroy(gameObject);
	}


}
