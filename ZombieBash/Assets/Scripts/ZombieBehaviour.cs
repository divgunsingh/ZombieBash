using UnityEngine;
using System.Collections;

public class ZombieBehaviour : MonoBehaviour {
	
		
		public float speed;
		public float jumpforce;
		public int Max_health=100;
		private int _health;// Use this for initialization

	void Update () {

				if (Max_health <= 0) {
						Destroy (this.gameObject);
				}
		}

	  void OnBite()
	{

		}
	void OnCollisionEnter(Collision other) {
				Destroy (other.gameObject);

				if (other.gameObject.tag == "player") {
						other.gameObject.SendMessage ("DecreaseHealth");
			
				}
				if (other.gameObject.tag == "Bullet") {
			Max_health-=10;
				}

		}
}

