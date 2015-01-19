using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {

	public float movingSpeed = 10f;
	public int health = 10;

	// Use this for initialization
	void Start () {
//		var _human = GetComponent<Human>();
	}
	
	// Update is called once per frame
	void Update () {
		var move = new Vector3 (1, 0, 0);

		rigidbody.AddForce (transform.TransformDirection(move)*movingSpeed);
		if(health <= 0)
			ChangeToZombie();

	}

	void ChangeToZombie(){
		Destroy (gameObject);
	}

	void decreaseHealth(){
			health -= 1;
	}

}
