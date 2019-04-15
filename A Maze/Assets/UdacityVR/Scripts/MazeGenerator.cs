using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    private const string NORTH_WALL = "North_Wall";
    private const string SOUTH_WALL = "South_Wall";
    private const string EAST_WALL = "East_Wall";
    private const string WEST_WALL = "West_Wall";

    public GameObject mazeBlockPrefab;
    public GameObject waypointParent;
    public GameObject waypointPrefab;
    public int mazeRows;
    public int mazeColumns;
    private GameObject[] maze;

	// Use this for initialization
	void Start () {
        maze = new GameObject[mazeRows * mazeColumns];
        BuildMaze(mazeRows, mazeColumns);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void BuildMaze(int rows, int columns) {
        InstantiateBlocks(rows, columns);
        BuildPaths(rows, columns);
    }

    private void InstantiateBlocks(int rows, int columns) {
        for (int i = 0; i < rows; i++)
        {
            float zOffset = (-4.0f * i) + 100.0f;
            for (int j = 0; j < columns; j++)
            {
                float xOffset = (4.0f * j) - (2.0f * (columns - 1));
                int blockIndex = (columns * i) + j;
                Vector3 position = new Vector3(xOffset, 0.0f, zOffset);
                maze[blockIndex] = Instantiate(mazeBlockPrefab, position, Quaternion.identity, gameObject.transform);
                Instantiate(waypointPrefab, position + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity, waypointParent.transform);
            }
        }
    }

    private void BuildPaths(int rows, int columns) {
        Stack<int> positionsStack = new Stack<int>();
        int numberOfBlocks = maze.Length;
        int currentBlockIndex = Random.Range(0, numberOfBlocks);
        GameObject currentBlock = maze[currentBlockIndex];
        List<int> visitedBlocks = new List<int>();
        visitedBlocks.Add(currentBlockIndex);
        while (visitedBlocks.Count < numberOfBlocks)
        {
            List<int> neighbours = GetNonVisitedNeighbours(currentBlockIndex, rows, columns, visitedBlocks);
            if (neighbours.Count > 0) {
                int nextBlockIndex = neighbours[Random.Range(0, neighbours.Count)];
                GameObject nextBlock = maze[nextBlockIndex];
                DisableWalls(currentBlock, nextBlock);
                positionsStack.Push(currentBlockIndex);
                currentBlock = nextBlock;
                currentBlockIndex = nextBlockIndex;
                visitedBlocks.Add(currentBlockIndex);
            } else {
                currentBlockIndex = positionsStack.Pop();
                currentBlock = maze[currentBlockIndex];
            }
        }

        CreateStartAndFinishBlocks(rows, columns);
    }

    private List<int> GetNonVisitedNeighbours(int blockIndex, int rows, int columns, List<int> visitedBlocks) {
        List<int> neighbours = new List<int>();

        int upIdx = blockIndex - columns;
        if ((upIdx >= 0) && (!visitedBlocks.Contains(upIdx))) {
            neighbours.Add(upIdx);
        }

        int downIdx = blockIndex + columns;
        if ((downIdx < rows * columns) && (!visitedBlocks.Contains(downIdx))) {
            neighbours.Add(downIdx);
        }

        int rightIdx = blockIndex + 1;
        if (((int)(rightIdx/columns) == (int)(blockIndex/columns)) && (!visitedBlocks.Contains(rightIdx))) {
            neighbours.Add(rightIdx);
        }

        int leftIdx = blockIndex - 1;
        if ((leftIdx >= 0) && (((int)(leftIdx/columns)) == (int)(blockIndex/columns)) && (!visitedBlocks.Contains(leftIdx))) {
            neighbours.Add(leftIdx);
        }

        return neighbours;
    }

    private void DisableWalls(GameObject currentBlock, GameObject nextBlock) {
        
        if (nextBlock.transform.position.z > currentBlock.transform.position.z) {
            currentBlock.transform.Find(SOUTH_WALL).gameObject.SetActive(false);
            nextBlock.transform.Find(NORTH_WALL).gameObject.SetActive(false);

        } else if (nextBlock.transform.position.z < currentBlock.transform.position.z) {
            currentBlock.transform.Find(NORTH_WALL).gameObject.SetActive(false);
            nextBlock.transform.Find(SOUTH_WALL).gameObject.SetActive(false);

        } else if (nextBlock.transform.position.x > currentBlock.transform.position.x) {
            currentBlock.transform.Find(WEST_WALL).gameObject.SetActive(false);
            nextBlock.transform.Find(EAST_WALL).gameObject.SetActive(false);

        } else {
            currentBlock.transform.Find(EAST_WALL).gameObject.SetActive(false);
            nextBlock.transform.Find(WEST_WALL).gameObject.SetActive(false);
        }
    }

    private void CreateStartAndFinishBlocks(int rows, int columns) {
        int startBlockIndex = (int)(columns / 2);
        maze[startBlockIndex].transform.Find(SOUTH_WALL).gameObject.SetActive(false);

        int finishBlockIndex = (columns * (rows - 1)) + startBlockIndex;
        maze[finishBlockIndex].transform.Find(NORTH_WALL).gameObject.SetActive(false);
    }
}
