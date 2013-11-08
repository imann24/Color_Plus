using UnityEngine;
using System.Collections;

public class cubeScript : MonoBehaviour {
	
	gameController activateScript;
	
	// Use this for initialization
	void Start () 
	{
		activateScript = GameObject.Find("GameController").GetComponent<gameController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnMouseDown ()
	{
	//reference to process in the "gameController" script: changes cube color, activates/deactivates cubes
		activateScript.processClickedCube(gameObject);
	}
}
