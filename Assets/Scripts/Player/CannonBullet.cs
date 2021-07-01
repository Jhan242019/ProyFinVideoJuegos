using System.Collections;
 using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour {

	public GameObject explosionEffect;
	public bool isIdle;

	private Rigidbody2D rigidBody2D;
	private bool cannonShot, isExplode; 

	void Awake(){
		rigidBody2D = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		cannonShot = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameplayController.instance.gameInProgress) {
			if (cannonShot && rigidBody2D.velocity.sqrMagnitude <= 0) {
				DestroyBullet ();
			}
		}
	}
		
	void DestroyBullet(){
		Destroy (gameObject);
		if (!isExplode) {
			isExplode = true;	
			GameObject newExplosionEffect = Instantiate (explosionEffect, transform.position, Quaternion.identity) as GameObject;
			Destroy (newExplosionEffect, 3f);
		}
	}
}
