using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script del manejador de sonidos de fondo
public class MusicController : MonoBehaviour {
	public static MusicController instance;

	
	public AudioClip background, gameplay, loseSound, winSound;

	[HideInInspector] 
	public AudioSource audioSource;

	

	void Awake(){
		CreateInstance ();
		InitializeVariables ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	
	void CreateInstance(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	void InitializeVariables(){
		audioSource = GetComponent<AudioSource> ();
	}

	
	public void PlayBgMusic(){
		if(background){
			audioSource.clip = background;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}

	public void GameplaySound(){
		if(gameplay){
			audioSource.clip = gameplay;
			audioSource.loop = true;
			audioSource.Play ();
		}
	}

	public void StopAllSounds(){
		if(audioSource.isPlaying){
			audioSource.Stop ();
		}
	}

}
