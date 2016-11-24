using UnityEngine;
using System.Collections;

public class EnemyGunRange : MonoBehaviour {

	public EnemyShootTriggerL enemyRange;

	public bool isLeft = false;

	void Awake()
	{
		enemyRange = gameObject.GetComponentInParent<EnemyShootTriggerL> ();
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.CompareTag("Player"))
		{
			if(isLeft)
			{
				enemyRange.Attack (false);
			}
			else
			{
				enemyRange.Attack (true);
			}
		}
	}
}
