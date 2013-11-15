using UnityEngine;
using System.Collections;

public class startScreen : MonoBehaviour {
	public GUIStyle myButtonSize;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI ()
	{
		myButtonSize.fontSize = 100;
		if(GUI.Button(new Rect(20,40,1000,1000), "Start (click here)", myButtonSize)) 
			{
				Application.LoadLevel("Main Game");
			}
	//display start button
	//when clicked, start button loads "GameScene"
	
	}
}
