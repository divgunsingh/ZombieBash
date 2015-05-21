using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof(CharacterController)), RequireComponent (typeof(Seeker))]

public class ZombieBehaviour : AIPath {
	
		
		public float speedZombie =5;
		public float jumpforce;
		public int MaxHealth=100;
		private int _health;
	public UiHubScript hub;
	public GameObject ZombieBullet;
	public float _timeDelay = 2f;
	private float _timeProgress = 0f;

	//AI REQUIREMENTS
	public float SleepVelocity = 0.4F;
	public float ChaseDistance = 5;
	

	public int FleeHealth = 1;
	private int _Zombiehealth;
	
	private EnemyState _currentState;
	
	// player stuff
	private List<PlayerController> _knownPlayers;
	private PlayerController _nearestPlayer;
	
	/* REFERENCES */
	public Text StatusText;
	public Slider HealthBar;
	
	enum EnemyState { Idle, Chasing, Fleeing}


	public void onDestroyedPlayer(){

		_knownPlayers = GameObject.FindGameObjectsWithTag("Player").
			Select(player => player.GetComponent<PlayerController>()).
				Where(behaviour => behaviour != null).ToList();

	}
	public void onNewPlayer(){
		_knownPlayers = GameObject.FindGameObjectsWithTag("Player").
			Select(player => player.GetComponent<PlayerController>()).
				Where(behaviour => behaviour != null).ToList();
	}
	public new void Start(){

		_knownPlayers = GameObject.FindGameObjectsWithTag("Player").
			Select(player => player.GetComponent<PlayerController>()).
				Where(behaviour => behaviour != null).ToList();

		_currentState = EnemyState.Idle;
		_health = MaxHealth;
		HealthBar.minValue = 0;
		HealthBar.maxValue = MaxHealth;
		HealthBar.value = _health;

		var nearbyPlayers = _knownPlayers.Where(player => Vector3.Distance(player.transform.position, transform.position) < ChaseDistance).ToList();
		if (nearbyPlayers.Any())
		{
			_nearestPlayer =
				nearbyPlayers.OrderBy(player => Vector3.Distance(player.transform.position, transform.position))
					.ToList()[0];
			if (_nearestPlayer != null)
				target = _nearestPlayer.transform;
		}
		
		base.Start();
	}

	private void UpdateStateInfo()
	{
		// update the closest player

		if (!_knownPlayers.Any()) {
			ChangeState(EnemyState.Idle);
			return;
		}

		_nearestPlayer = _knownPlayers.OrderBy(player => Vector3.Distance(player.transform.position, transform.position)).ToList()[0];
		
		/* IF THERE ARE ANY NEARBY PLAYERS WE EITHER FLEE OR CHASE */
		if (Vector3.Distance(_nearestPlayer.transform.position, transform.position) < ChaseDistance)
			// if our health is too low, then we flee
			ChangeState(_health <= FleeHealth ? EnemyState.Fleeing : EnemyState.Chasing);
		else
			// all else considered, we idle
			ChangeState(EnemyState.Idle);
	}


	protected new void Update () {

		_timeProgress += Time.deltaTime;


				if (MaxHealth <= 0) {
						Destroy (this.gameObject);
				}
		UpdateStateInfo();
		
		switch (_currentState)
		{
		case EnemyState.Chasing:
			target = _nearestPlayer.transform;
			HandleChase();

			if (_timeProgress > _timeDelay) {
				shoot();
				_timeProgress = 0;
			}

			break;
		case EnemyState.Fleeing:
			// we very simply run away
			// nothing fancy, just trying to stay clear of the player
			if (_nearestPlayer != null)
				controller.SimpleMove(speedZombie * (transform.position - _nearestPlayer.transform.position).normalized);
			break;
		case EnemyState.Idle:
			//Move Towards Player BAse
		//	var pbob= GameObject.FindGameObjectWithTag("PlayerBase");
			//var zbob= GameObject.FindGameObjectWithTag("ZombiesBase");
//			controller.SimpleMove(speedZombie * ( pbob.transform.position -transform.position).normalized);

			//controller.SimpleMove(speedZombie * ( zbob.transform.position -transform.position).normalized);
			break;
		}
		}
	private void HandleChase()
	{
		//Calculate desired velocity
		var dir = CalculateVelocity(tr.position);

		
		//Rotate towards targetDirection (filled in by CalculateVelocity)
		RotateTowards(targetDirection);
		dir.y = 0;
		
		controller.SimpleMove(dir);
	}

	private void ChangeState(EnemyState newState)
	{
		_currentState = newState;
		StatusText.text = newState.ToString();
	}

	public void shoot(){
		
		
		var bullet = Instantiate (ZombieBullet) as GameObject;
		bullet.transform.position = transform.position;
		bullet.GetComponent<Rigidbody>().velocity = (targetDirection*100);

		
		
	}

	public void die(){

	
	
	}
	void OnCollisionEnter(Collision other) {
		Debug.Log("Player Collided");
		
		if (other.gameObject.tag == "Player") {

          hub.health -=1;
			Debug.Log(hub.health);
				}
				if (other.gameObject.tag == "Bullet") {
			MaxHealth-=10;
				}

		}
}

