using UnityEngine;
using System.Collections;

public class cubeScript : MonoBehaviour {
	
	gameController activateScript;
	public bool inActive;
	
	// Use this for initialization
	void Start () 
	{
		activateScript = GameObject.Find("GameController").GetComponent<gameController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.transform.renderer.material.color == Color.grey)
		{
			inActive = true;
		}
	}
	
	void OnMouseDown ()
	{
	//reference to process in the "gameController" script: changes cube color, activates/deactivates cubes
		if (inActive == false)
		{
			activateScript.processClickedCube(gameObject);
		}
	}
}
