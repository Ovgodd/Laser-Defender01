using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDeal : MonoBehaviour
{

	[SerializeField] int damage = 100;


	public int GetDamage()
	{
		return damage;
	}

	public void Hit()
	{
		Destroy(gameObject);
	}



}
