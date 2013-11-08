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
	public float mainTimer;
	public float countdownTimer = 60;
	public bool plusFormation;
	public bool activeCube = false;
	public bool destroyedCube = false;
	public bool keyPressedDown = false;
	public static bool gameWin;
	public float cubeXPoint;
	public float cubeYPoint;
	
	// Use this for initialization
	void Start () 
	{
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
		GUI.Box(new Rect(10,10,100,30), "Score:" + totalScore.ToString());
		GUI.Box(new Rect(375, 315, 100, 30), "Next Cube" );
		GUI.Box(new Rect(10, 60, 100, 30), "Timer:" + (int) countdownTimer);
		
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
			if (keyPressedDown == false)
			{
				Destroy(cubes [Random.Range(0,7), Random.Range (0,4)]);	
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
		if (checkNumKeys() && keyPressedDown == false)	
		{
			cubes[randomColumn(), int.Parse(Input.inputString)].transform.renderer.material.color = nextCubeColor;	
			keyPressedDown = true;
			
		}
	}
	public bool checkNumKeys(){
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
	public bool detectPlus ()
	{	
		for (int x = 1; x < (numCubesHor-1); x++)
		{
			for (int y = 1; y < (numCubesVert-1); y++)
			{
				if (cubes [x, y].transform.renderer.material.color == cubes [x+1, y+1].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x-1, y-1].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x+1, y-1].transform.renderer.material.color&&
					cubes [x, y].transform.renderer.material.color == cubes [x-1, y+1].transform.renderer.material.color)
					{
						plusFormation = true;	
					}
				else 
					{
						plusFormation = false;
					}
				}
			}
		return plusFormation;
	}
	
	//detects when the cubes are aligned in a "+" formation (same color, or all colors)
	//returns true if this arrangment is found	
	
	//public void processClickedCube (GameObject ClickedCube, Color  colorPassedIn)
	public void processClickedCube(GameObject clickedCube)
	{
		if (clickedCube.transform.renderer.material.color != Color.magenta && activeCube)
		{
			clickedCube.transform.renderer.material.color = tempColor;
			cubes[(int) cubeXPoint/2, (int) cubeYPoint/2].transform.renderer.material.color = Color.white;
			activeCube = false;
		}
		
		else if (clickedCube.transform.renderer.material.color == Color.magenta)
		{
			clickedCube.transform.renderer.material.color = tempColor;
			activeCube = false;
		}
		
		else
		{
			tempColor = clickedCube.transform.renderer.material.color;
			cubeXPoint = clickedCube.transform.position.x;
			cubeYPoint = clickedCube.transform.position.y;
			clickedCube.renderer.material.color = Color.magenta;
			activeCube = true;
		}
	}
	
	//chooses and random number (1-5) and returns it (to assign the y coordinate of where "NextCube" moves to	
	public int randomColumn ()
	{
		randomNumber = Random.Range(0,8);
		return randomNumber;
	}
		
	//chooses a random number in the color array and returns a color 
	public Color changeColor () 
	{
		return cubeColor[Random.Range(0,4)];	
	}
	
	public int keypadInput ()
	{
		for (int i = 1; i < limitKeyNum; i++)
		{
			if (Input.GetKeyDown(numInput[i]))
			{
				keypadInputNum = i;
			}
		}
		return keypadInputNum;
	}
}
