using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script will update the score in the scene.
public class Score : MonoBehaviour {

	
	public Text secondsSurvived; // reference to the secondsSurvived UI element
	
	// Update is called as the computer draws the frame
	void Update () {

		if (FindObjectOfType<GameManager>().gameEnded != true){ //makes sure the game has not ended. If ended, no need to update score.
			Debug.Log(Time.timeSinceLevelLoad);
			secondsSurvived.text = string.Format("{0:#,0.0}", Time.timeSinceLevelLoad); //the UI element is updated with the time since level started.
		}
		
		
	}
}
