using UnityEngine;
using System.Collections;
using Leap;

public class TitleSceneController : MonoBehaviour {

	Controller leapController;

	int count = 0;

	// Use this for initialization
	void Start () {
		leapController = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
		if (count == -1)
			return;

		using (Frame frame = leapController.Frame())
		{
			//	if found hands, 'count' do countup.
			if (frame.Hands.Count > 0)
			{
				count++;
			}
			else
			{
				count = 0;
			}
			//	Transition scene from this scene to BattleScene(GameScene).
			if (count > 60)
			{
				count = -1;
				Application.LoadLevel(1);	//	To Battle Scene
			}
		}
	}

	void OnDestroy() {

	}
}
