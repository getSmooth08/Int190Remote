using UnityEngine;
using System.Collections;

public class RockCameraShake : MonoBehaviour {

	//attach camera parent with shake script in order to call camera shake
	public GameObject camShaker;

	//adjust camera shake during run time ().ShakeCamera(shakeAmount, shakeDuration);
	public float shakeAmount = 0f;
	public float shakeDuration = 0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void CameraShakeTrigger()
	{
		camShaker.GetComponent<CameraShake02> ().ShakeCamera(shakeAmount, shakeDuration);
	}
}
