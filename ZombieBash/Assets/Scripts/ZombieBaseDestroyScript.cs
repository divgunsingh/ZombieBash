using UnityEngine;
using System.Collections;

public class ZombieBaseDestroyScript : MonoBehaviour {

	int hit = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("Collision with Zombie Base");
		if (hit == 0)
						DestroyZombieBase ();


		else if (collision.gameObject.tag == "Player")
	       hit = hit - 1;

		else if (collision.gameObject.tag == "Bullet")
			hit = hit - 1;
	}



	void DestroyZombieBase()
	{
		Destroy (this.gameObject);
	}
}
