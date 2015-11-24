using UnityEngine;
using System.Collections;

public class GridCreator : MonoBehaviour
{
	#region Fields and Vars

	public GameObject line;

	public int numXSquare, numYSquares;
	private int numXLines, numYLines;
	
	public int YLimitLine;

	public int percentageOfScreenSpace;

	[Range(0f, 4f)]
	public float lineThicknessFactor;

	public static GameObject[,] gridBlocks {get; private set;}
	public static Vector2[,] gridPositions {get; private set;}

	public static float gridSquareWidth {get; private set;}
	private float gridXStartPos;
	private float gridYStartPos;

	public CustomAspectRatio gridAspectRatio;

	public delegate void GridEvents();
	public static event GridEvents gridCreated = delegate {};

	#endregion

	#region Grid Creation

	private void CreateGrid()
	{
		gridAspectRatio = new CustomAspectRatio(numXSquare, numYSquares, percentageOfScreenSpace);
		gridSquareWidth = gridAspectRatio.useWidth / gridAspectRatio.xRatio;
		
		gridBlocks = new GameObject[numXSquare, numYSquares];
		gridPositions = new Vector2[numXSquare, numYSquares];

		CalcGridPositions();
		DrawLines();

		if(YLimitLine >= numYLines)
			Debug.Log("Y Limit Line is greater than the number of Y lines in the grid.");

		//call the event  that the grid has been created
		gridCreated();
	}

	private void CalcGridPositions()
	{
		float halfGridSquareWidth = gridSquareWidth/2;
		
		float gridXStartPos = (-gridAspectRatio.useWidth / 2);
		float gridYStartPos = (-gridAspectRatio.useHeight / 2);
		
		for(int x = 0; x < gridPositions.GetLength(0); x++)
		{
			for(int y = 0; y < gridPositions.GetLength(1); y++)
			{
				gridPositions[x, y] = new Vector2(gridXStartPos + halfGridSquareWidth + (gridSquareWidth * x),
				                                  gridYStartPos + halfGridSquareWidth + (gridSquareWidth * y));
			}
		}
	}

	private void DrawLines()
	{
		GameObject gridLineRoot = new GameObject();
		gridLineRoot.name = "Grid Lines";
		gridLineRoot.transform.parent = gameObject.transform;
		gridLineRoot.transform.localScale = Vector3.one;

		numXLines = numXSquare + 1;
		numYLines = numYSquares + 1;

		//Calc thickness of lines based on screen resolution
		float lineThickness = (1f + ((1f / gameObject.GetComponent<RectTransform>().localScale.x)/100f)) * lineThicknessFactor;

		for (int x = 0; x < numXLines; x++)
		{
			Vector2 lineSize = new Vector2(lineThickness, gridAspectRatio.useHeight);
			Vector2 position = new Vector2((-gridAspectRatio.useWidth/2) + (x * gridSquareWidth),
			                               0f);
			InstantiateLineAtPosition(gridLineRoot, "Line X " + x, position, lineSize, false);
		}

		for (int y = 0; y < numYLines; y++)
		{
			Vector2 lineSize = new Vector2(gridAspectRatio.useWidth, lineThickness);
			Vector2 position = new Vector2(0f, (-gridAspectRatio.useHeight/2) + (y * gridSquareWidth));
			bool limitLine = false;

			//Check if this is the limit line
			if(y == YLimitLine)
			{
				limitLine = true;
			}

			InstantiateLineAtPosition(gridLineRoot, "Line X " + y, position, lineSize, limitLine);
		}
	}
	
	private void InstantiateLineAtPosition(GameObject parent, string lineName, Vector2 position, Vector2 lineSize, bool isLimitLine)
	{
		//set up the line GameObject
		GameObject lineObj = Instantiate(line, Vector3.zero, Quaternion.identity) as GameObject;
		lineObj.name = lineName;
		lineObj.transform.SetParent(parent.transform);

		//Assign size and position
		RectTransform rectTranf = lineObj.GetComponent<RectTransform>();
		rectTranf.transform.localPosition = new Vector3(position.x, position.y, 0f);
		rectTranf.sizeDelta = lineSize;
		rectTranf.localScale = Vector3.one;

		//Assign colour if limit line
		if(isLimitLine)
		{
			lineObj.GetComponent<UnityEngine.UI.Image>().color = Color.red;
		}
	}

	#endregion

	#region Grid Position Control

	public bool isGridPositionFree(int x, int y)
	{
		return gridBlocks[x, y] == null;
	}
	
	public void SetGridPositionAsTaken(int x, int y, GameObject block)
	{
		gridBlocks[x, y] = block;
	}
	
	public void FreeGridPositions(int[] x, int[] y)
	{
		for(int i = 0; i < x.Length; i++)
		{
			FreeGridPosition(x[i], y[i]);
		}
	}

	public void FreeGridPosition(int x, int y)
	{
		gridBlocks[x, y] = null;
		Destroy(gridBlocks[x, y]);
	}

	#endregion

	#region MonoBs

	void Start () 
	{
		CreateGrid();
	}

	#endregion
}