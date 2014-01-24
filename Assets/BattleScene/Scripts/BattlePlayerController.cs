using UnityEngine;
using System.Collections;
using Leap;

public class BattlePlayerController : MonoBehaviour {
	Controller leapController = new Controller();
	BattlePlayerMagic playerMagic = new BattlePlayerMagic();
	public ParticleSystem chargeEffect;

	MoverPlayerController moverController;

	public GameObject fireBall;
	public GameObject iceBall;
	public GameObject thunderBall;


	enum PlayerState {
		Move,	//	one hand
		Magic,	//	two hand
		Sword,	//	one tool
		None,
	}


	PlayerState nowPlayerState = PlayerState.None;
	int stateCoolTime = 0;

	const int MAX_STATE_COOL_TIME = 120;


	// Use this for initialization
	void Start () {
		playerMagic.SetChargeEffect(chargeEffect);
		playerMagic.ShotMagic = new BattlePlayerMagic.ShotMagicDelegate(ShotMagic);
		moverController = GetComponent<MoverPlayerController>();
		moverController.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = leapController.Frame();

		if (stateCoolTime > 0)
			stateCoolTime--;
		else
			UpdatePlayerState(frame);
	
		switch (nowPlayerState)
		{
			case PlayerState.Magic:
				playerMagic.UpdateFrame(frame);
				break;
		}

	}

	void UpdatePlayerState(Frame frame)
	{
		if (frame.Hands.Count == 0)
			ChangePlayerState(PlayerState.None);

		if (frame.Hands.Count == 1) {
			ChangePlayerState(PlayerState.Move);
			moverController.enabled = true;
		}
		else
			moverController.enabled = false;

		if (frame.Hands.Count == 2)
			ChangePlayerState(PlayerState.Magic);
		else
			playerMagic.ResetState();
	}

	void ChangePlayerState(PlayerState state)
	{
		if (nowPlayerState == state)
			return;
		stateCoolTime = MAX_STATE_COOL_TIME;
		if (nowPlayerState == PlayerState.Magic)
			iTween.MoveBy(GameObject.Find("Hands"), new Vector3(0, -2), 0.5f);
		else if (state == PlayerState.Magic)
			iTween.MoveBy(GameObject.Find("Hands"), new Vector3(0, 2), 0.5f);


		nowPlayerState = state;
	}



	//	called by BattlePlayerMagic
	public void ShotMagic(BattlePlayerMagic.MagicState magicType)
	{
		GameObject magicBall;
		switch (magicType)
		{
		case BattlePlayerMagic.MagicState.MagicFire:
			magicBall = Instantiate(fireBall, transform.position, transform.rotation) as GameObject;
			break;
		case BattlePlayerMagic.MagicState.MagicIce:
			magicBall = Instantiate(iceBall, transform.position, transform.rotation) as GameObject;
			break;
		case BattlePlayerMagic.MagicState.MagicThunder:
			magicBall = Instantiate(thunderBall, transform.position, transform.rotation) as GameObject;
			break;
		}

	}


}

public class BattlePlayerMagic {
	public enum MagicState
	{
		Waiting,
		Charging,
		
		MagicFire,
		MagicIce,
		MagicThunder,
	}
	
	int[] POWER_TABLE =
	{
	//	0,   1,   2     3     4      5
		0, 20, 120, 200, 300, 1000, 
	};
	
	int [] PARTICLE_TABLE =
	{
		0, 10, 15, 200, 500, 1000,
	};
	
	Color[] MAGIC_COLOR_TABLE =
	{
		Color.red,		//	fire
		Color.blue,		//	ice
		Color.yellow,	//	thunder
	};

	public delegate void ShotMagicDelegate(MagicState magicType);
	public ShotMagicDelegate ShotMagic;

	int magicPower = 0;
	MagicState nowMagicState = MagicState.Waiting;
	
	public ParticleSystem chargeEffect;
	
	public MagicState GetState()
	{
		return nowMagicState;
	}
	
	public void SetChargeEffect(ParticleSystem effect)
	{
		chargeEffect = effect;
	}
	
	public int GetPower()
	{
		for (int i = 0; i < POWER_TABLE.Length; i++)
		{
			//if (POWER_TABLE[i] < 
			if (POWER_TABLE[i] > magicPower)
				return i;
		}
		return POWER_TABLE.Length;	//	test code
	}
	
	public void UpdateFrame(Frame frame) {
		//	check Leap can see 2 hands.
		for (int i = 0; i < 2; i++)
		{
			if (!frame.Hands[i].IsValid)
			{
				this.ResetState();
				return;
			}
		}
		
		Vector rightHand, leftHand;
		if (frame.Hands[0].PalmPosition.x < frame.Hands[1].PalmPosition.x)
		{
			rightHand = frame.Hands[1].PalmPosition;
			leftHand = frame.Hands[0].PalmPosition;
		}
		else
		{
			rightHand = frame.Hands[0].PalmPosition;
			leftHand = frame.Hands[1].PalmPosition;
		}
		Debug.Log("right:  " + rightHand);
		Debug.Log("left :  " + leftHand);
		
		switch(nowMagicState)
		{
		case MagicState.Waiting:
			if (IsRangeInside(rightHand.y, leftHand.y, 30) &&
				IsRangeInside(rightHand.z, leftHand.z, 30) &&
				rightHand.z > 0 && leftHand.z > 0)
			{
					Debug.Log("State to magic charging.");
					ChangeState(MagicState.Charging);
			}
			break;
		case MagicState.Charging:
			ChargeMagicPower();	//	Charge Proccess
			//	fire
			if (IsRangeInside(rightHand.y, leftHand.y, 60) &&
				(rightHand.z - leftHand.z < -60))
			{
					ChangeState(MagicState.MagicFire);
				break;
			}
			//	Ice
			if (IsRangeInside(rightHand.y, leftHand.y, 60) &&
				(rightHand.z - leftHand.z > 60))
			{
					ChangeState(MagicState.MagicIce);
				break;
			}
			if (IsRangeOutside(rightHand.x, leftHand.x, 230))
			{
				ChangeState(MagicState.MagicThunder);
			}
			break;
		default:	//	about Magic group
			//	Shot Magic
			if (rightHand.z < 0 && leftHand.z < 0)
			{
				if (GetPower() > 2)
					ShotMagic(nowMagicState);
				ResetState();
			}
			break;
		}
	}
	
	public void ResetState() {
		magicPower = 0;
		nowMagicState = MagicState.Waiting;
		chargeEffect.emissionRate = 0;
		chargeEffect.startColor = Color.white;
	}
	
	void ChangeState(MagicState state) {
		nowMagicState = state;
		switch (nowMagicState) {
		case MagicState.MagicFire:
			chargeEffect.startColor = MAGIC_COLOR_TABLE[0];
			break;
		case MagicState.MagicIce:
			chargeEffect.startColor = MAGIC_COLOR_TABLE[1];
			break;
		case MagicState.MagicThunder:
			chargeEffect.startColor = MAGIC_COLOR_TABLE[2];
			break;
		}
	}
	
	void ChargeMagicPower() {
		magicPower++;
		Debug.Log("power: " + GetPower());
		chargeEffect.emissionRate = PARTICLE_TABLE[GetPower()];
	}
	
	bool IsRangeInside(float param1, float param2, float range) {
		float distance = Mathf.Abs(param1 - param2);
		if (distance < range)
			return true;
		return false;
	}
	bool IsRangeOutside(float param1, float param2, float range) {
		float distance = Mathf.Abs(param1 - param2);
		if (distance > range)
			return true;
		return false;
	}
	
			
}
