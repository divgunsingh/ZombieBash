using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


enum EnemyState { insane, changetozombie, hungry , scared,help}

[RequireComponent (typeof(CharacterController)), RequireComponent (typeof(Seeker))]

public class Human : AIPath {
	
	public float movingSpeed = 10f;
	public int health = 10;

	public float SleepVelocity = 0.4F;
	public float ChaseDistance = 5;
	
	public int MaxHealth = 3;
	public int FleeHealth = 1;
	private int _health;
	
	private EnemyState _currentState;
	
	// human stuff
	//private List<PlayerController> _knownPlayers;
	private List<ZombieBehaviour> _knownZombies;
	//private PlayerController _nearestPlayer;
	private ZombieBehaviour _nearestZombie;
	
	/* REFERENCES */
	public Text StatusText;
	public Slider HealthBar;

	// Use this for initialization
	public new void Start () {   //o lculate the relation of distance b/w the human and zombies
		_knownZombies = GameObject.FindGameObjectsWithTag("Zombie").
			Select(zombie => zombie.GetComponent<ZombieBehaviour>()).
				Where(behaviour => behaviour != null).ToList();

		_currentState = EnemyState.hungry;
		_health = MaxHealth;
		HealthBar.minValue = 0;
		HealthBar.maxValue = MaxHealth;
		HealthBar.value = _health;

		var nearbyZombies = _knownZombies.Where(zombie => Vector3.Distance(zombie.transform.position, transform.position) < ChaseDistance).ToList();
		if (nearbyZombies.Any())
		{
			_nearestZombie =
				nearbyZombies.OrderBy(zombie => Vector3.Distance(zombie.transform.position, transform.position))
					.ToList()[0];
			if (_nearestZombie != null)
				target = _nearestZombie.transform;
		}
		
		base.Start();
	}

	private void UpdateStateInfo()
	{
		// update the closest player
		_nearestZombie =  _knownZombies.OrderBy(player => Vector3.Distance(player.transform.position, transform.position)).ToList()[0];
		
		/* IF THERE ARE ANY NEARBY PLAYERS WE EITHER FLEE OR CHASE */
		if (Vector3.Distance (_nearestZombie.transform.position, transform.position) < 0) {
						// if our health is too low, then we flee
		ChangeState (_health <= FleeHealth ? EnemyState.help : EnemyState.scared);
				} 

		else if (Vector3.Distance (_nearestZombie.transform.position, transform.position) < 5) {
		
			//Human Change to Zombie 
			ChangeState(EnemyState.changetozombie);

		
		}
		else
			// all else considered, we idle
			ChangeState(EnemyState.hungry);
	}

	private void ChangeState(EnemyState newState)
	{
		_currentState = newState;
		StatusText.text = newState.ToString();
	}

	protected new void Update () {

		UpdateStateInfo();

		// enum EnemyState { insane, changetozombie, hungry , scared,Fleeing}
		switch (_currentState)
		{
		case EnemyState.insane:
			target = _nearestZombie.transform;
			HandleChase();

			break;
		case EnemyState.help:
			// we very simply run away
			// nothing fancy, just trying to stay clear of the player
			if (_nearestZombie != null)
				controller.SimpleMove(speed * (transform.position - _nearestZombie.transform.position).normalized);

			break;
		case EnemyState.hungry:
			// we do nothing
			target = _nearestZombie.transform;
			HandleChase();

			break;
		case EnemyState.scared:
			// we do nothing

			break;
		case EnemyState.changetozombie:
			// we do nothing
			gameObject.GetComponent<ZombieBehaviour>().enabled = true;
			gameObject.GetComponent<Human>().enabled=false;
			gameObject.GetComponent<Renderer>().material.color = Color.red;
			gameObject.tag = "Zombie";
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
	
	
	
	
}
