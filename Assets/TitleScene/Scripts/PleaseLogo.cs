using UnityEngine;
using System.Collections;

public class PleaseLogo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.FadeTo(gameObject, iTween.Hash("alpha", 0.1f, "time", 1.0f, "looptype", iTween.LoopType.pingPong));
	}
	
	// Update is called once per frame
	void Update () {
	
	}





}
