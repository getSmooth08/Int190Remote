using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public bool invisible = false;

	public Transform target;

	public Animator animator;

	public AudioClip hit;

	[System.Serializable]
	public class EnemyStats{
		public int Health = 100;
	}

	void Start(){

		GetComponent<AudioSource> ().playOnAwake = false;
		GetComponent<AudioSource> ().clip = hit;
		invisible = false;
	}

	void Update(){

		//var damp = 5;
		// thest 2 if statements make the enemies face the 'player'
		if(target.transform.position.x > transform.position.x){
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		if(target.transform.position.x < transform.position.x){
			GetComponent<SpriteRenderer> ().flipX = true;
		}


/*		if(Mathf.Abs (target.position.x - transform.position.x) > float.Epsilon)
		{
			GetComponent<SpriteRenderer> ().flipX = true;
		}*/
/*		if(target != null)
		{
			GetComponent<SpriteRenderer> ().flipX = true;
		}
		else
		{
			GetComponent<SpriteRenderer> ().flipX = false;
		}*/


		if(invisible == true){
			GetComponent<SpriteRenderer> ().color = Color.red;
			invisible = false;
		}
		else if(invisible == false)
		{
			GetComponent<SpriteRenderer> ().color = Color.white;
		}
	}

	public EnemyStats stats = new EnemyStats ();

	public void Damage(int damage)
	{
	
		invisible = true;
		stats.Health -= damage;
		animator.SetTrigger ("enemyHit");
		GetComponent<AudioSource> ().Play ();
		//GetComponent<SpriteRenderer> ().color = Color.red;
		if(stats.Health <= 0)
		{
			
			Destroy (gameObject);
			GetComponent<AudioSource> ().Play ();
		}
	}


}
