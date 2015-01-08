using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private bool _isGrounded;
	public GameObject  BulletPrefab;
	public float speed = 10;
	public float jumpForce = 100;
	public float  BulletForce=100;
	public Quaternion AimDirection;
		// Use this for initialization
	void Start () {
		_isGrounded = false;
	
	}
	
	void FixedUpdate() {
		if (!_isGrounded)
						return;

		if (Input.GetButtonDown ("Fire1"))
						shoot ();
		if (Input.GetButton ("Fire2"))
			shoot ();

		var targetVelocity = new Vector3(Input.GetAxis ("Horizontal"),0,Input.GetAxis ("Vertical"));

		rigidbody.AddForce (transform.TransformDirection(targetVelocity)*speed);
		if (Input.GetButtonDown ("Jump")&& _isGrounded)
						rigidbody.AddForce (new Vector3(0,jumpForce,0));
	}

	public void OnCollisionStay(){
		Debug.Log ("On Collision Stay");
		_isGrounded = true;
	}
	public void shoot(){
		var bullet = Instantiate (BulletPrefab) as GameObject;

		bullet.transform.position = transform.position;

		bullet.rigidbody.velocity = (AimDirection* new Vector3(0,0,BulletForce));


	}
}
