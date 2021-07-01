using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public GameObject settingsPanel, exitPanel;
	public Toggle soundToggle;

	// Use this for initialization
	void Start () {
		if(GameController.instance != null && MusicController.instance != null){
			if (GameController.instance.estaEncendidaMusica) {
				MusicController.instance.PlayBgMusic ();
				soundToggle.isOn = true;
			} else {
				MusicController.instance.StopAllSounds ();
				soundToggle.isOn = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if (exitPanel.activeInHierarchy) {
				exitPanel.SetActive (false);
			} else {
				exitPanel.SetActive (true);
			}

			if (settingsPanel.activeInHierarchy) {
				settingsPanel.SetActive (false);
			}

		}
	}

	public void PlayButton(){
		SceneManager.LoadScene("Level Menu");
	}

	public void SettingsButton(){
		settingsPanel.SetActive (true);
	}

	public void CloseSettingsButton(){
		settingsPanel.SetActive (false);
	}

	public void SoundToggle(){
		if (soundToggle.isOn) {
			GameController.instance.estaEncendidaMusica = true;
			MusicController.instance.PlayBgMusic ();
			GameController.instance.Save ();
		} else {
			GameController.instance.estaEncendidaMusica = false;
			MusicController.instance.StopAllSounds ();
			GameController.instance.Save ();
		}
	}

	public void YesButton(){
		Application.Quit ();
	}
		

	public void NoButton(){
		exitPanel.SetActive (false);
	}
}
