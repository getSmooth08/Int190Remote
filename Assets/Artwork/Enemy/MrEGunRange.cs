using UnityEngine;
using System.Collections;

public class MrEGunRange : MonoBehaviour {

	public MrEShootTrigger enemyRange;
	public MrEShootTrigger2 enemyRange2;

	public bool isLeft = false;

	void Awake()
	{
		enemyRange = gameObject.GetComponentInParent<MrEShootTrigger> ();
		enemyRange2 = gameObject.GetComponentInParent<MrEShootTrigger2> ();
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
