using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//This script manages the end of the game and the restart
public class GameManager : MonoBehaviour {


public bool gameEnded = false; // public variable which will signal when the game has ended. initialised to false.

public void EndGame() // function is called to end game and restart
{

	if (gameEnded == false){ //  checking the bool value to make sure game hasnt already ended.
	gameEnded = true; // bool value set to true since game has ended
	Invoke ("Restart", 4f);// this function will invoke the restart function after 4 seconds.
	}


}

void Restart(){ // Function will restart the game

	SceneManager.LoadScene (SceneManager.GetActiveScene().name); //The main scene will be reloaded to restart the game.
}


}