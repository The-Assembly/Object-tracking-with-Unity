using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script will ensure the blocks are destroyed after they pass the player and go below the scene to ensure there is no memory used
unnecessarily to store blocks that are not being used anymore*/

public class Block : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		if (transform.position.y < -2f)  // if the position of the block is below -2 on the y-coordinate
		{
			Destroy(gameObject); // the gameObject is destroyed and removed from the scene using this function
		}
	}

}