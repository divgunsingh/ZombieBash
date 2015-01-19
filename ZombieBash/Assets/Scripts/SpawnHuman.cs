using UnityEngine;
using System.Collections;

public class SpawnHuman : MonoBehaviour {


	public GameObject human_Prefab;
	public float _timeDelay = 2f;
	private float _timeProgress = 0f;
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
		_timeProgress += Time.deltaTime;
		if (_timeProgress > _timeDelay) {
			GenHuman ();
			_timeProgress = 0;
		}
	}
	
	void GenHuman(){
		var gen_Human = Instantiate(human_Prefab) as GameObject;
		gen_Human.transform.position = transform.position;
	}
}
