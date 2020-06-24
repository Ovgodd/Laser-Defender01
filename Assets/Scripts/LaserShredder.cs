using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserShredder : MonoBehaviour


{

	// Start is called before the first frame update
	private void OnTriggerEnter2D(Collider2D collision)

	{
	
		Destroy(collision.gameObject);

	}
}
