using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiHubScript : MonoBehaviour {
	public Slider HealthBar;
	public Slider EnergyBar;
	public Text   ScoreText;

	public Slider PlayerHealthBarSlider;

	// Use this for initialization
	public int MaxHealth = 5;
	public int health;
	
	public int score;
	
	public int MaxEnergy=100;
	public int energy;


	public GameObject PlayerHealthBar;
	public void Awake(){

		PlayerHealthBar=GameObject.FindGameObjectWithTag ("Player");
//		PlayerHealthBarSlider=PlayerHealthBar.GetComponent<Slider>();

	}

	public void Update(){



	}

}
