using UnityEngine;
using System.Collections;
using Leap;

public class LeapTest : MonoBehaviour {

    Controller leapController;

	// Use this for initialization
	void Start () {
	    leapController = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = leapController.Frame();

        Finger finger = frame.Fingers[0];
        Vector pos = finger.TipPosition;
        Debug.Log("x = " + pos.x + "   y = " + pos.y + "   z = " + pos.z);

	}
}
