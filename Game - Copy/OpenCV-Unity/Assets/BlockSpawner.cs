using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will spawn the obstacle blocks throughtout the game
public class BlockSpawner : MonoBehaviour {

	public Transform[] spawnPoints; // stores an array of the positions of the spawnpoints

	public GameObject blockPrefab;  // refernce to the block that will be spawned

	public float timeBetweenWaves = 3f; // time between two simultaneous set of blocks falling

	private float timeToSpawn = 2f;  // time for the blocks to spawn ; initial time is set to 2 seconds

	void Update () //called everytime the computer draws a new frame
	{

		//if (FindObjectOfType<GameManager>().gameEnded != true){ // checks if the game has not ended
			
			
				if (Time.time >= timeToSpawn) // if the time taken since game started is greater than the spawn time, then blocks are spawned.
				{
					SpawnBlocks();  // call to the function which will spawn blocks
				 /*time to spawn is updated to current time + 3 seconds. 
				 This means, the next wave will be 3 seconds after the first wave of blocks spawned*/
					timeToSpawn = Time.time + timeBetweenWaves;
				}

	   // }
	}

/* This function spawns the blocks. It will spawn blocks at all spawnpoints (array) except one, 
which is the spot the player will have to evade the wave of blocks. */
	void SpawnBlocks () 
	{
		// a random value is generated and stored. Random number will be between 0 and number of spawnpoints.
		// blocks will be spawned at all points in the spawnpoints array except at the random integer point.
		int randomIndex = Random.Range(0, spawnPoints.Length); 
		
			//looping over all spawn points. Spawnpoints is an array.
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				if (randomIndex != i) // if the spawn point is not equal to the random number generated above
				{
				Instantiate(blockPrefab, spawnPoints[i].position, Quaternion.identity); // blocks are instantiated. It clones the original block and returns the clone.
				}
			

		 }
		
	}
	
}