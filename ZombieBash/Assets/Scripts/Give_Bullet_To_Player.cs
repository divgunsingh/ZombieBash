using UnityEngine;
using System.Collections;

public class Give_Bullet_To_Player : MonoBehaviour {

	PlayerController _player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player") {
			Debug.Log("Collision occurs here ");
			var gob =GameObject.FindGameObjectWithTag("Bullet");
			GameObject.Destroy(gob);


			_player  = GetComponent<PlayerController>();
			_player.BulletPrefab.GetComponent<ParticleSystem>().startColor= Color.red;
			//GameObject.Destroy(gameobject);
			  //makes the object invisible
		}
	}
}
