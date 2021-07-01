using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	public int puntosMamixosGolpe;
	public float contadorDanio;
	public GameObject efectoMuerte;
	public AudioClip herido;

	private Animator animator;
	private int puntosGolpe;
	private Vector3 rebote;
	private int scoreMaximo;

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		InitializeVariables ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeVariables(){
		puntosGolpe = puntosMamixosGolpe;
		scoreMaximo = 1000;
	}

	void UpdateAnimationState(){
		if(puntosGolpe <= 5){
			animator.SetTrigger ("isDamage");
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.relativeVelocity.magnitude > contadorDanio)
		{
			puntosGolpe -= Mathf.RoundToInt(collision.relativeVelocity.magnitude);
			UpdateScoreStatus (Mathf.RoundToInt (collision.relativeVelocity.magnitude));
			if(GameController.instance != null && MusicController.instance != null){
				if(GameController.instance.estaEncendidaMusica)
				{
					if (gameObject != null) {
						AudioSource.PlayClipAtPoint (herido, transform.position);
					}
				}
			}
		}
			

		UpdateAnimationState ();

		if(puntosGolpe <= 0){
			Death ();

			if(collision.gameObject.CompareTag("Player Bullet")){
				rebote = collision.transform.GetComponent<Rigidbody2D> ().velocity;
				rebote.y = 0f;
				collision.transform.GetComponent<Rigidbody2D> ().velocity = rebote;
			
			}
		}
	}

	void Death(){
		Destroy (gameObject);
		GameObject newDeathEffect = Instantiate (efectoMuerte, transform.position, Quaternion.identity) as GameObject;
		Destroy (newDeathEffect, 3f);
		if(GameController.instance != null){
			GameController.instance.puntaje += scoreMaximo;
		}
		DisplayScore ();

	}

	void UpdateScoreStatus(int hitScore){
		if(GameController.instance != null){
			GameController.instance.puntaje += hitScore; 
		}
	}

	void DisplayScore(){
		GameObject scoreText = Instantiate (Resources.Load ("Score Text Canvas"), new Vector3(transform.position.x, transform.position.y + 1f), Quaternion.identity) as GameObject;
		scoreText.transform.GetChild (0).transform.GetComponent<Text> ().text = scoreMaximo.ToString ();
		Destroy (scoreText, 2f);
	}
}
