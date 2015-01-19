using UnityEngine;
using System.Collections;

public class GoalBehaviour : MonoBehaviour
{
	public Material DeactivatedMaterial;
	
	public void Deactivate()
	{
		GetComponentInChildren<Light>().enabled = false;
		GetComponentInChildren<Canvas>().enabled = false;
		//transform.GetChild(1).gameObject.SetActive(false);
		renderer.material = DeactivatedMaterial;
		gameObject.tag = "dead_goal";
	}
}
