using UnityEngine;
using System.Collections;

public class PlayerBaseDetroy : MonoBehaviour {
	int hit =3 ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("Collision with Zombie Base");
		if (hit <= 0)
			DestroyPlayerBase ();
		
		
		if (collision.gameObject.tag == "Zombie")
			hit = hit - 1;
	}
	
	void DestroyPlayerBase()
	{
		Destroy (this.gameObject);
	}
}
