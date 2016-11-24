using UnityEngine;
using System.Collections;

public class energyScript : MonoBehaviour {
	public float bulletSpeed = 5f;
	public float lifetime = 10f;
	public LayerMask ignoreLayers;
	private float timeAlive = 0f;
	private Rigidbody2D rb;
	public float damageAmount = 1f;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2 (Mathf.Sign(transform.right.x)*bulletSpeed,0);
		timeAlive += Time.deltaTime;
		if(timeAlive > lifetime)
			Destroy (gameObject);
	}
	void OnTriggerEnter2D(Collider2D other){
		//Replace with a raycast later
		if(((1<<other.gameObject.layer) & ignoreLayers) == 0)//It wasn't in an ignore layer
		{
			other.gameObject.SendMessage ("Damage", damageAmount,SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);

		}
	}
}
