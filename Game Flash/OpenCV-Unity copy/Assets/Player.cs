using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;

// This script takes care of the player movement
public class Player : MonoBehaviour
{
    


    public float mapWidth = 5f; //to choose how wide map is
    private Rigidbody2D rb;  // to get a reference to the rigidbody in Unity
    private float xPos;   // stores the x-coordinate received from the socket
    private Vector2 newPos; //stores the new position (both x and y coordinate for the player)

    Thread receiveThread;   //The thread that is used for the UDP socket communication
    UdpClient client;       // UDP client that will be used to communicate with the python script
    public int port = 5065; // port number that will be used for the socket communication
    private bool endThread = false;


    void Start() //called once at the beginning of the script
    {
        newPos = new Vector2(0.0f, 0.0f); // initialising position of the player at the start of the game
        rb = GetComponent<Rigidbody2D>(); //intialising above variable with a reference to the rigidbody from the Unity
        
        init(); //function call to start the thread that will receive data from the python script.
    }


    void Update() //called everytime the computer draws a frame
    {
     
        
        //assigning the received x-coordinate to x-coordinate of variable newPos
        // x-coordinate is clamped between certain values, so that the player access is within a certain range of the screen
        newPos.x = Mathf.Clamp(xPos, -mapWidth, mapWidth); 

        rb.MovePosition(newPos); // this function receives the new position and moves the player to the updated positions


    }


    void OnCollisionEnter2D() //Function executed when the player collides with something. 
    {
        
            
            if (FindObjectOfType<GameManager>().gameEnded != true){
                
                endThread= true;    
                FindObjectOfType<GameManager>().EndGame();// calls the EndGame() function in GameManager to end the game
            }
            
           

        }

       



    private void init() // This function takes care of initialising the variables needed for the threads and starting the thread
    {

        // a new thread is initialised. ThreadStart will hold a reference to the argument function "ReceiveData" that will execute on thread
        receiveThread = new Thread(new ThreadStart(ReceiveData)); 
        receiveThread.IsBackground = true; //ensures thread runs in background
        receiveThread.Start(); //starts the thread
      


    }



    private void ReceiveData() //method that executes on the thread
    {
        client = new UdpClient(port); //initialises the UDP client with the port 5065
         //represents a network endpoint as IP address and port number
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        while (!endThread)
        {
            try
            {
               
                // Blocks until a message returns on this socket from a remote host.
                byte[] data = client.Receive(ref anyIP);
                //The received data is stored as text
                string text = Encoding.UTF8.GetString(data);
               
                //The text is converted to float.
				xPos = float.Parse(text);
                //Debug.Log(xPos);
             


            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }

        
    }


    void OnApplicationQuit() //this function (built in Unity) runs when the game is on pause or quits. The below code will affect all gameobjects.
    {
       

            receiveThread.Abort();// if the thread is not null, it needs to be aborted before the game ends.
            if (client !=null){
              client.Close();  
            }
                
            Debug.Log(receiveThread.IsAlive); //must be false
        
    }



}
