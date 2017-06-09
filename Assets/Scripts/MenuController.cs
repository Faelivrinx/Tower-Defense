using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	[SerializeField]
	private GameObject mainMenuPanel;

	[SerializeField]
	private GameObject chooseLevelPanel;

	[SerializeField]
	private Object [] maps;

	[SerializeField]
	Text mapName;

	private int mapIndex = 0;

	void Start(){
		
		mainMenuPanel.SetActive (true);
		chooseLevelPanel.SetActive (false);

	}

	public void setMainMenu() {
		
	}


	public void setPlayMenu() {
		
	}

	public void playButtonPressed(){
		mainMenuPanel.SetActive (false);
		chooseLevelPanel.SetActive (true);
	}

	public void mainMenuButtonPressed(){
		mainMenuPanel.SetActive (true);
		chooseLevelPanel.SetActive (false);
	}

	public void rightArrowPressed() {
		if (mapIndex < (maps.Length - 1)) {
			mapIndex++;
			setMap (mapIndex);
		}
	}

	public void leftArrowPressed() {
		if (mapIndex > 0) {
			mapIndex--;
			setMap (mapIndex);
		}
	}

	private void setMap(int index){
		Debug.Log (index);
		mapName.text = maps[index].ToString();
	}

	public void acceptMapPressed() {
		LevelManager.Instance.LoadLevel (mapIndex+1);
	}

	public void exit(){
		Application.Quit ();	
	}
		
}
