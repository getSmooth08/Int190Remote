using UnityEngine;
using System.Collections;

public class EnemyShootTriggerR : MonoBehaviour {

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
	
	}

	void OnTriggerStay2D(Collider2D col)
	{
		bulletTimer += Time.deltaTime;

		if(col.CompareTag("Player") && bulletTimer >= shootInterval)
		{
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointRight.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				//bulletTimer = 0;
		}
	}
}
