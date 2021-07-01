using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit2D(Collider2D collider){
		if (collider.CompareTag ("Player Bullet") || collider.CompareTag ("Object") || collider.CompareTag ("Enemy")) {
			Destroy (collider.gameObject);
		}
	}
}
