using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
	public GameObject [,] cubes;
	public GameObject nextCube;
	public Color [] cubeColor;
	public KeyCode [] numInput;
	public Color tempColor;
	public Color nextCubeColor;
	public int numCubesHor = 8;
	public int numCubesVert = 5;
	public int numColors = 5;
	public float turnTime = 7;
	public int nextCubeNum = 1;
	public int limitKeyNum = 6;
	public GameObject WhiteCube; 
	public GameObject NextCube;
	public static int totalScore;
	public int randomNumber;
	public int keypadInputNum;
	public int lowTime = 11;
	public int numWhiteCubes;
	public int foundX;
	public int foundY;
	public float mainTimer;
	public float countdownTimer = 600;
	public bool plusFormation = false;
	public bool activeCube = false;
	public bool destroyedCube = false;
	public bool keyPressedDown = false;
	public static bool gameWin;
	public float cubeXPoint;
	public float cubeYPoint;
	public GUIStyle myButtonColor;
	public static Input inputString;
	
	// Use this for initialization
	void Start () 
	{
		numWhiteCubes = numCubesHor * numCubesVert;
		cubes = new GameObject[numCubesHor, numCubesVert];
		for (int x = 0; x < numCubesHor; x++)
		{
			for (int y = 0; y < numCubesVert; y++)
			{
				cubes [x, y] = (GameObject) Instantiate (WhiteCube, new Vector3 (x * 2, y * 2, 0), Quaternion.identity);
				cubes [x, y].renderer.material.color = Color.white;
			}						
		}
		
		nextCube = (GameObject) Instantiate (NextCube, new Vector3 (7, -2, 0), Quaternion.identity);
		
		nextCube.transform.renderer.material.color = Color.green;
		nextCubeColor = Color.green;
		
		cubeColor = new Color[numColors];
		cubeColor [0] = Color.black;
		cubeColor [1] = Color.blue;
		cubeColor [2] = Color.green;
		cubeColor [3] = Color.red;
		cubeColor [4] = Color.yellow;
		
		numInput = new KeyCode[limitKeyNum];
		numInput [0] = KeyCode.Keypad0;
		numInput [1] = KeyCode.Keypad1;
		numInput [2] = KeyCode.Keypad2;
		numInput [3] = KeyCode.Keypad3;
		numInput [4] = KeyCode.Keypad4;
		numInput [5] = KeyCode.Keypad5;	
	}
		
	void OnGUI () 
	{
		// Make a background box
		if (countdownTimer < lowTime)
			{
				myButtonColor.normal.textColor = Color.red;
			}
		GUI.Box(new Rect(10,10,100,30), "Score:" + totalScore.ToString());
		GUI.Box(new Rect(1050, 675, 100, 30), "Next Cube" );
		GUI.Box(new Rect(10, 60, 100, 30), "Timer:" + (int) countdownTimer, myButtonColor);

	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//the game and turn timers
		countdownTimer -= Time.deltaTime;
		mainTimer += Time.deltaTime;


		if (mainTimer > turnTime)
		{
			//destroy cubes method
			destroyACube();		
			//reset the ability to press a key for the next turn
			keyPressedDown = false;
			//get a random color for next cube
			nextCube.transform.renderer.material.color = changeColor();
			//place next cube's color into the random color distribution method: triggered upon keypad input
			nextCubeColor = nextCube.transform.renderer.material.color;
			//reset turn timer to 0
			mainTimer = 0;
		}
		//trigger the victory condition, if score has increased (to be displayed on the endScreen)
		if (totalScore > 0)
		{
			gameWin = true;
		}	
		//checks for number input and haven't already pressed a key
		if (checkNumKeys() && keyPressedDown == false)	
		{
			//checks whether the random cube is null
			// Commented Out: if (cubes[randomColumn(), getIndexFromString()] != null)

			//if not it assigns the "next cube" color to it
			findAValidCube(randomColumn(), getIndexFromString()).transform.renderer.material.color = nextCubeColor;	
			numWhiteCubes --;
			keyPressedDown = true;
		}
		if (countdownTimer < 0)
		{
			Application.LoadLevel("endScreen");	
		}
		if (processPlus())
		{
			totalScore += 10;	
		}		
	}

	public int getIndexFromString ()
	{
		int numKeyPressed;
		if (Input.inputString != null)
		{
			numKeyPressed = int.Parse(Input.inputString);
			return numKeyPressed - 1;
		}
		else
		{
			return 0;
		}
	}	
	//returns true if any of the number keys 1-5 are pressed
	public bool checkNumKeys()
	{
		if (Input.inputString == "1")
		{
			return true;	
		}
	if (Input.inputString == "2")
		{
		return true;
		}
	if (Input.inputString == "3")
		{
		return true;	
		}
	if (Input.inputString == "4")
		{
		return true;
		}
	if (Input.inputString == "5")
		{
		return true;	
		}
	

		return false;

	}
	
	public bool detectRainbowPlus (int x, int y) 
	{
		return false;		
	}
	
	//detecting a "+" of all the same color 
	public bool processPlus()
	{	
		//set valid cube values back to null
		foundX = -1;
		foundY = -1;
		//search array of cubes (exlcuding the edges)
		for (int x = 1; x < numCubesHor-1; x++)
		{
			for (int y = 1; y < numCubesVert-1; y++)
			{
				//check the cube if it is not currently white or grey
				if (cubes[x,y].transform.renderer.material.color != Color.white && cubes[x,y].transform.renderer.material.color != Color.grey)
				{
					//call the script that detects a solid "+"
					if (detectSolidColorPlus(x, y))
					{
						foundX = x;
						foundY = y;
					}
					
				}
			}
		}
		
		// if we found a valid cube "+"
		if (foundX != -1)
		{
			//make them grey
			greyOutCubes ();
			return true;
		}
		//if not
		else 
		{
			//do not return true; do not increase score
			return false;	
		}
	}
	
	//detects when the cubes are aligned in a "+" formation (same color, or all colors)
	//returns true if this arrangment is found	
	
	// accepts a gameObject from using raycasting
	public void processClickedCube(GameObject clickedCube)
	{
		//if the cube is white and another cube is active
		if (clickedCube.transform.renderer.material.color == Color.white && activeCube)
		{
			//set the white cube to this cubes color & set activeCube back to white (and make it normal size again)
			clickedCube.transform.renderer.material.color = tempColor;
			cubes[(int) cubeXPoint/2, (int) cubeYPoint/2].transform.localScale = new Vector3(1f, 1f, 1f);
			cubes[(int) cubeXPoint/2, (int) cubeYPoint/2].transform.renderer.material.color = Color.white;
			//communicate that there cube is no longer an active cube
			activeCube = false;
		}
		//if you click again on activeCube, its color remains the same and the cube deactivates 
		else if (clickedCube.transform.position.x == (float) cubeYPoint && clickedCube.transform.position.y == (float) cubeYPoint)
		{
			clickedCube.transform.renderer.material.color = tempColor;
			activeCube = false;
		}
		//if you click on a color cube and there is no activeCube, this cube becomes activeCube and it increases in size to show this
		else if (clickedCube.transform.renderer.material.color != Color.white && activeCube == false)
		{
			tempColor = clickedCube.transform.renderer.material.color;
			cubeXPoint = clickedCube.transform.position.x;
			cubeYPoint = clickedCube.transform.position.y;
			clickedCube.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			activeCube = true;
		}
		//make the cube its own color, ie. do nothing
		else
		{
			clickedCube.transform.renderer.material.color = clickedCube.transform.renderer.material.color;
		}
	}
	
	//chooses and random number (1-5) and returns it (to assign the y coordinate of where "NextCube" moves to	
	public int randomColumn ()
	{
		randomNumber = Random.Range(0,numCubesHor);
		return randomNumber;
	}
		
	//chooses a random number in the color array and returns a color 
	public Color changeColor () 
	{
		//return cubeColor[Random.Range(0,numCubesVert)];	
		return cubeColor[0];	
	}
	//locates a white cube or returns null
	public bool checkValid (int x, int y)
	{
		if(cubes[x,y].transform.renderer.enabled && cubes[x,y].transform.renderer.material.color == Color.white)
			return true;

		else 
		{
			return false;
		}
	}
	//greys out the cube at the foundX and foundY location in the array, in addition to the rest of the cubes in the "+" formation
	public void greyOutCubes ()
	{
		cubes [foundX, foundY].transform.renderer.material.color = Color.grey;
		cubes [foundX-1, foundY].transform.renderer.material.color = Color.grey;
		cubes [foundX+1, foundY].transform.renderer.material.color = Color.grey;
		cubes [foundX, foundY+1].transform.renderer.material.color = Color.grey;
		cubes [foundX, foundY-1].transform.renderer.material.color = Color.grey;


	}

	//returs true if all the cubes in the "+" formation are the same color as the cube at the center of the formation
	public bool detectSolidColorPlus (int x, int y)
	{
		if (cubes [x, y].transform.renderer.material.color == cubes [x+1, y].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x-1, y].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x, y-1].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x, y+1].transform.renderer.material.color)
			{
				return true;
			}
		else
		{
			return false; 	
		}
	}
	//returns a gameObject, if findAWhiteCube is able to locate one, only functions while numWhiteCubes is more than one
	public GameObject findAValidCube (int x, int y)
		{	
		int xCheck = x;
		int yCheck = y;
		GameObject validCube = null;
		// keep trying as long as there is at least one white cube
			while (validCube == null && numWhiteCubes > 0) 
			{
				if (checkValid(xCheck,yCheck))
				{
					validCube = cubes[xCheck,yCheck];
				}

				else
				{
					//reset x and y Check based on same random conditions initial x and y have
					xCheck = randomColumn();
					yCheck = getIndexFromString();
				}
			}
			return validCube;
		}
	public void destroyACube ()
	{
		// if no key was pressed during the turn
		if (keyPressedDown == false)
		{
			// try and find a random white cube to destroy
			GameObject cubeToDestroy = null;
			cubeToDestroy = findAValidCube (Random.Range(0, numCubesHor), Random.Range(0, numCubesVert));
			// keep trying as long as there is at least one white cube
			
			// if we found a valid white cube
			if (cubeToDestroy != null) {
				cubeToDestroy.renderer.enabled = false;
				numWhiteCubes--;
			}
			else 
			{
				// end the game
				Application.LoadLevel("endScreen");	
			}
		}
	}
}
