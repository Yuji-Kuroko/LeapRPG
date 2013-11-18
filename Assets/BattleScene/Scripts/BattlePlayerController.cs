using UnityEngine;
using System.Collections;
using Leap;

public class BattlePlayerController : MonoBehaviour {
	Controller leapController = new Controller();
	BattlePlayerMagic playerMagic = new BattlePlayerMagic();
	public ParticleSystem chargeEffect;

	public GameObject fireBall;
	public GameObject iceBall;
	public GameObject thunderBall;

	// Use this for initialization
	void Start () {
		playerMagic.SetChargeEffect(chargeEffect);
		playerMagic.ShotMagic = new BattlePlayerMagic.ShotMagicDelegate(ShotMagic);
			

	}
	
	// Update is called once per frame
	void Update () {
		Frame frame = leapController.Frame();
		playerMagic.UpdateFrame(frame);
	}


	public void ShotMagic(BattlePlayerMagic.MagicState magicType)
	{
		GameObject magicBall;
		switch (magicType)
		{
		case BattlePlayerMagic.MagicState.MagicFire:
			magicBall = Instantiate(fireBall) as GameObject;
			break;
		case BattlePlayerMagic.MagicState.MagicIce:
			magicBall = Instantiate(iceBall) as GameObject;
			break;
		case BattlePlayerMagic.MagicState.MagicThunder:
			magicBall = Instantiate(thunderBall) as GameObject;
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
		0, 20, 60, 180, 300, 1000, 
	};
	
	int [] PARTICLE_TABLE =
	{
		0, 10, 50, 200, 500, 1000,
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
			}
			//	Ice
			if (IsRangeInside(rightHand.y, leftHand.y, 60) &&
				(rightHand.z - leftHand.z > 60))
			{
					ChangeState(MagicState.MagicIce);
			}
			break;
		default:	//	about Magic group
			//	Shot Magic
			if (rightHand.z < 0 && leftHand.z < 0)
			{
				ShotMagic(nowMagicState);
				ResetState();
			}
			break;
		}
	}
	
	void ResetState() {
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
