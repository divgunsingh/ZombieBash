using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : Photon.MonoBehaviour {
//	private bool _isGrounded;
	public GameObject  BulletPrefab;
	public float speed = 10;
	public float jumpForce = 200;
	public float  BulletForce=100;
	public Quaternion AimDirection;
	private List<SuperPower1> _powerTypes;
	private int _currentlyUsedPower = 0;


	/* SUPER POWERS*/
	//vmware fusion7
	
	public bool _icePower= false;
	public bool _firePower= false;
	public bool _bulletPower= true;
	/* Ui Reference*/


	public UiHubScript UiHub;
	public Slider playerHealthBar;
	private Vector3 _accuratePosition;
	private Quaternion _accurateRotation;
	
	public GameObject UiObject;


	void Awake(){

		UiObject=	GameObject.FindGameObjectWithTag ("UIHUB");
		UiHub = UiObject.GetComponent<UiHubScript>();

		}


	void Start () {
//		_isGrounded = false;
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (var gameobject in allObjects) {
				
			gameobject.SendMessage("onNewPlayer",SendMessageOptions.DontRequireReceiver);
		}
		_powerTypes = new List<SuperPower1> {
			new SuperPower1{
				powerDamage = 0, 
				powerColor = Color.yellow,
				
				powersEffect = "",
				powersEnergyCost = 0,
				powersShakeModidifier = 0,
				powersSpeedModifier = 0,
				powersRangeModifier = 0,
				powersFired = 0,
				
				changeForChildren = 0,
				changeForChildrenGenerationFriction = 0,
				maxChild = 0,
				minChild = 0
			},
			new SuperPower1{
				powerDamage = 0, 
				powerColor = Color.blue,
				
				powersEffect = "",
				powersEnergyCost = 0,
				powersShakeModidifier = 0,
				powersSpeedModifier = 0,
				powersRangeModifier = 0,
				powersFired = 0,
				
				changeForChildren = 0,
				changeForChildrenGenerationFriction = 0,
				maxChild = 0,
				minChild = 0
			},
			new SuperPower1{
				powerDamage = 0, 
				powerColor = Color.red,
				
				powersEffect = "",
				powersEnergyCost = 0,
				powersShakeModidifier = 0,
				powersSpeedModifier = 0,
				powersRangeModifier = 0,
				powersFired = 0,
				
				changeForChildren = 0,
				changeForChildrenGenerationFriction = 0,
				maxChild = 0,
				minChild = 0
			}
		};

		UiHub.HealthBar.maxValue = UiHub.MaxHealth;
		UiHub.HealthBar.minValue = 0;

		UiHub.EnergyBar.maxValue = UiHub.MaxEnergy;
		UiHub.EnergyBar.minValue = 0;

		playerHealthBar.maxValue = UiHub.MaxHealth;
		playerHealthBar.minValue = 0;

		UiHub.energy = UiHub.MaxEnergy;
		UiHub.EnergyBar.value = UiHub.energy;
        
		//player world hud
		//UiHub.PlayerHealthBarSlider.maxValue = UiHub.MaxEnergy;
		//UiHub.PlayerHealthBarSlider.minValue = 0;

		UiHub.health = UiHub.MaxHealth;
		UiHub.HealthBar.value = UiHub.health;
		playerHealthBar.value = UiHub.health;

		UiHub.score = 0;
		UiHub.ScoreText.text = UiHub.score.ToString();

	//	var players = GameObject.FindGameObjectsWithTag("Player");
	/*
		foreach (var player in players)
		{
			Debug.Log("requesting an identity broadcast");
			//player.SendMessage("BroadcastIdentity");
		}*/
	}

	public void Update(){
//		_isGrounded = false;
		if (photonView.isMine) {
						HandleInput ();
			
			if (Input.GetButtonDown ("Fire1"))
				shoot ();
			
			if (Input.GetButton ("Fire2")) {
				rigidbody.velocity = Vector3.up * 3f;
			}
			
			if (Input.GetKeyDown (KeyCode.O))
				_currentlyUsedPower--;
			if (Input.GetKeyDown(KeyCode.P))
				_currentlyUsedPower++;
			
			if (_currentlyUsedPower > _powerTypes.Count)
				_currentlyUsedPower = _powerTypes.Count - 1;
			
			if (_currentlyUsedPower < 0)
				_currentlyUsedPower = 0;


			if (UiHub.energy >= UiHub.MaxEnergy){ 
				
				UiHub.energy = UiHub.MaxEnergy; 
			}

			UiHub.energy += 1;
			UiHub.EnergyBar.value = UiHub.energy; 
			playerHealthBar.value= UiHub.health; 
			UiHub.HealthBar.value= UiHub.health; 
			//UiHub.PlayerHealthBarSlider.value=UiHub.energy;
	
				

				}
		else
			Interpolate();
	}

	private void HandleInput()
	{
		var targetVelocity = new Vector3(Input.GetAxis ("Horizontal"),0,Input.GetAxis ("Vertical"));
		
		rigidbody.AddForce (transform.TransformDirection(targetVelocity)*speed);
		
		if(Input.GetButtonDown("Fire1"))
			shoot();

		if (Input.GetButtonDown ("Jump")) {
						rigidbody.AddForce (new Vector3 (0, jumpForce, 0));
				}
	}
	
	private void Interpolate()
	{
		rigidbody.position = Vector3.Lerp(rigidbody.position, _accuratePosition, Time.deltaTime * 5f);
		rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, _accurateRotation, Time.deltaTime * 5f);
	}



	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(rigidbody.position);
			stream.SendNext(rigidbody.rotation);
		}
		else
		{
			_accuratePosition = (Vector3) stream.ReceiveNext();
			_accurateRotation = (Quaternion) stream.ReceiveNext();
		}
	}
	[RPC]
	public void shoot(){


		var bullet = Instantiate (BulletPrefab) as GameObject;
		bullet.transform.position = transform.position;
		bullet.rigidbody.velocity = (AimDirection* new Vector3(0,0,BulletForce));


		var bulletModel = _powerTypes[_currentlyUsedPower];
		bullet.particleSystem.startColor = bulletModel.powerColor;
		bullet.renderer.material.color = bulletModel.powerColor;

		AdjustEnergy (10);

	
	}


	private void AdjustHealth()
	{
		UiHub.health -=5 ;
		Debug.Log (UiHub.health);
		playerHealthBar.value= UiHub.health; 
		UiHub.HealthBar.value = UiHub.health;
		// check if dead; clamp health behind maxhealth
		if (UiHub.health < 1)
			deletePlayer();
		else if (UiHub.health > UiHub.MaxHealth)
			UiHub.health = UiHub.MaxHealth;
		

	}
	
	private void AdjustScore(int delta)
	{
		UiHub.score += delta;
	
	}
	[RPC]
	private void AdjustEnergy(int delta)
	{
		UiHub.energy -= delta;


	}

	public void deletePlayer(){
		
		Destroy (this.gameObject);
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (var gameobject in allObjects) {
			
			gameobject.SendMessage("onDestroyedPlayer",SendMessageOptions.DontRequireReceiver);
		}
	}
	
	private void Die()
	{
		// TODO: implement
	}

	public void OnCollisionStay(Collision collision){
	
//		_isGrounded = true;

		if (collision.gameObject.tag == "goal")
		{
			collision.gameObject.SendMessage("Deactivate");

		}
	}
	
}
