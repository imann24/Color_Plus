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
	public float turnTime = 2;
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
	public float countdownTimer = 60;
	public bool plusFormation = false;
	public bool activeCube = false;
	public bool destroyedCube = false;
	public bool keyPressedDown = false;
	public static bool gameWin;
	public float cubeXPoint;
	public float cubeYPoint;
	public GUIStyle myButtonColor;
	
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
		
		countdownTimer -= Time.deltaTime;
		mainTimer += Time.deltaTime;
		//check for the "+" arrangment of cubes
		//spawn "NextCube"
		//destroy a bock if no number input is pressed 
		//number key input
		//ends game if timer expires
		//ends game if there are no available cubes 
		//upon end game, load "endScreen" (different "Load Level") 
		//turn loop
		if (mainTimer > turnTime)
		{
			// if no key was pressed during the turn
			if (keyPressedDown == false)
			{
				// try and find a random white cube to destroy
				GameObject cubeToDestroy = null;
				cubeToDestroy = findAValidCube (Random.Range(0, numCubesHor), Random.Range(0, numCubesVert));
				/*// keep trying as long as there is at least one white cube
				while (cubeToDestroy == null && numWhiteCubes > 0) {
					cubeToDestroy = findACube(Random.Range(0,numCubesHor), Random.Range (0,numCubesVert));
				}
				*/
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
			keyPressedDown = false;
			nextCube.transform.renderer.material.color = changeColor();
			nextCubeColor = nextCube.transform.renderer.material.color;
			mainTimer = 0;
		}
		
		if (totalScore > 0)
		{
			gameWin = true;
		}	
	
		/*string keypadInput = Input.inputString();
		print (int.parse(keypadInput));*/
		
		//checks for number input and whether a color has already been distributed this turn
		if (checkNumKeys() && keyPressedDown == false)	
		{
			//checks whether the random cube is null
			// Commented Out: if (cubes[randomColumn(), getIndexFromString()] != null)

			//if not it assigns the "next cube" color to it
			findAValidCube(randomColumn(), getIndexFromString()).transform.renderer.material.color = nextCubeColor;	
			keyPressedDown = true;
		}
		if (countdownTimer < 0)
		{
			Application.LoadLevel("endScreen");	
		}
		processPlus();
		if (processPlus())
		{
			totalScore += 5;	
		}		
	}
	

	public int getIndexFromString ()
	{
		int numKeyPressed = int.Parse(Input.inputString);
		return numKeyPressed - 1;
	}	

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
	else 
		{
		return false;
		}
	}
	
	public bool detectRainbowPlus (int x, int y) 
	{
		return false;		
	}
	
	//detecting a "+" of all the same color 
	public bool processPlus()
	{	
		//check each cube
		foundX = -1;
		foundY = -1;
		for (int x = 1; x < numCubesHor-1; x++)
		{
			for (int y = 1; y < numCubesVert-1; y++)
			{
				if (cubes[x,y].transform.renderer.material.color != Color.white)
				{
					if (detectSolidColorPlus(x, y))
					{
						foundX = x;
						foundY = y;
					}
					
				}
			}
		}
		
		// if we found a valid cube
		if (foundX != -1)
		{
			//make them grey
			greyOutCubes ();
			return true;
		}
		else 
		{
			return false;	
		}
	}
	
	//detects when the cubes are aligned in a "+" formation (same color, or all colors)
	//returns true if this arrangment is found	
	
	//public void processClickedCube (GameObject ClickedCube, Color  colorPassedIn)
	public void processClickedCube(GameObject clickedCube)
	{
		if (clickedCube.transform.renderer.material.color == Color.white && activeCube)
		{
			clickedCube.transform.renderer.material.color = tempColor;
			cubes[(int) cubeXPoint/2, (int) cubeYPoint/2].transform.localScale = new Vector3(1f, 1f, 1f);
			cubes[(int) cubeXPoint/2, (int) cubeYPoint/2].transform.renderer.material.color = Color.white;
			activeCube = false;
		}
		
		else if (clickedCube.transform.position.x == (float) cubeYPoint && clickedCube.transform.position.y == (float) cubeYPoint)
		{
			clickedCube.transform.renderer.material.color = tempColor;
			activeCube = false;
		}
		else if (clickedCube.transform.renderer.material.color != Color.white && activeCube == false)
		{
			tempColor = clickedCube.transform.renderer.material.color;
			cubeXPoint = clickedCube.transform.position.x;
			cubeYPoint = clickedCube.transform.position.y;
			clickedCube.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			activeCube = true;
		}
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
		return cubeColor[Random.Range(0,numCubesVert)];	
	}
	public GameObject findACube (int x, int y)
	{
	
		if (cubes[x,y].transform.renderer.enabled && cubes[x,y].transform.renderer.material.color == Color.white)
		{
			return cubes[x,y];
		}
		else 
		{
			return null;
		}
	}
	public void greyOutCubes ()
	{
		cubes [foundX, foundY].transform.renderer.material.color = Color.grey;
		cubes [foundX-1, foundY-1].transform.renderer.material.color = Color.grey;
		cubes [foundX+1, foundY-1].transform.renderer.material.color = Color.grey;
		cubes [foundX-1, foundY+1].transform.renderer.material.color = Color.grey;
		cubes [foundX+1, foundY+1].transform.renderer.material.color = Color.grey;
	}
	
	public bool detectSolidColorPlus (int x, int y)
	{
		if (cubes [x, y].transform.renderer.material.color == cubes [x+1, y+1].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x-1, y-1].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x+1, y-1].transform.renderer.material.color&&
			cubes [x, y].transform.renderer.material.color == cubes [x-1, y+1].transform.renderer.material.color)
			{
				return true;
			}
		else
		{
			return false; 	
		}
	}
	public GameObject findAValidCube (int x, int y)
		{	
		GameObject validCube = null;
		// keep trying as long as there is at least one white cube
			while (validCube == null && numWhiteCubes > 0) 
				{
				validCube = findACube(x, y);
				}
			if (validCube != null)
				{
					return validCube;
				}
			else 
				{
					return null;
				}
		}
}
