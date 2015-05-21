using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour {

	public GameObject power_up_prefab;
	public GameObject bullet;
	PlayerController _player;

	void Update()
	{

	}

	void Start()
	{
		SpawnPowerUp ();
	}

	void SpawnPowerUp()
	{
		Vector3 pos = new Vector3 (this.transform.position.x, this.transform.position.y * (-1), this.transform.position.z);
	    bullet =  Instantiate(power_up_prefab,this.transform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<ParticleSystem>().enableEmission = false;
			// var power_bullet = PlayerController._powerTypes[Random.Range(0,3)];


	}

	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && bullet!=null) {

			bullet.transform.position = _player.transform.position;
			_player.BulletPrefab = bullet;
		}

	}*/

}
