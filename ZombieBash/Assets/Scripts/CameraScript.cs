using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public GameObject Target;
	public float CameraDistance=1;
	public float CameraXDelta=1;
	public float CameraYDelta=1;
	public float CameraSpeed=10;
	private float _cameraXRotation;
	private float _cameraYRotation;
	private float MinYRotation=-20;
	private float MaxYRotation=90;

	public void FixedUpdate(){
		_cameraXRotation += Input.GetAxis ("Mouse X")*CameraSpeed;
		_cameraYRotation -= Input.GetAxis ("Mouse Y")*CameraSpeed;

		_cameraYRotation=ClampAngle (_cameraYRotation,MinYRotation,MaxYRotation);
		var rotation = Quaternion.Euler(new Vector3(_cameraYRotation,_cameraXRotation,0));


		transform.rotation=rotation;

		if(Target==null){
			return;
		}


				transform.position=Target.transform.position + transform.TransformDirection(new Vector3(CameraXDelta,CameraYDelta,CameraDistance));
			//transform.position=Target.transform.position;     		
			Target.transform.rotation=Quaternion.Euler(0,_cameraXRotation,0);
			var player=Target.GetComponent<PlayerController>();
			if(player!=null)
				player.AimDirection=rotation;

	

	}
	private static float ClampAngle(float angle, float min,float max){
		if (angle < -360F)
						angle += 360F;
		if (angle > 360F)
						angle -= 360F;
		return Mathf.Clamp (angle,min,max);


	}
}
