using UnityEngine;
using System.Collections;

public class EnemyShootTriggerL : MonoBehaviour {

	public GameObject bullet;
	public float bulletSpeed = 100f;
	public float bulletTimer = 1f;
	public float shootInterval = 25f;


	public Transform target;

	public Transform 	shootPointLeft,shootPointRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	//shots per second is down but executing everyframe not when in bounds
/*
		bulletTimer += Time.deltaTime;

		if(bulletTimer >= shootInterval)
		{
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;
		}
	*/
	}

	public void Attack(bool attackingRight)
	{

		bulletTimer += Time.deltaTime;

		if(bulletTimer >= shootInterval){
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

			if(!attackingRight){
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;
			}

			if(attackingRight){
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointRight.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;
			}
		}
	}
/*
	void OnTriggerStay2D(Collider2D col)
	{
		bulletTimer += Time.deltaTime;

		if(col.CompareTag("Player") && bulletTimer >= shootInterval)
		{
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				//bulletTimer = 0;
		}
	}
*/
}
