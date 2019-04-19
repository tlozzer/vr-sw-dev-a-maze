using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    private const string NORTH_WALL = "North_Wall";
    private const string SOUTH_WALL = "South_Wall";
    private const string EAST_WALL = "East_Wall";
    private const string WEST_WALL = "West_Wall";

    public GameObject mazeBlockPrefab;
    public GameObject waypointsParent;
    public GameObject waypointPrefab;
    public GameObject keyPrefab;
    public GameObject coinsParent;
    public GameObject coinPrefab;
    public int mazeRows;
    public int mazeColumns;
    public int numberOfItensInMaze;

    private GameObject[] maze;

	// Use this for initialization
	void Start () {

        // Maze Rows must be between 5 and 15;
        if (mazeRows < 5)
            mazeRows = 5;
        else if (mazeRows > 15)
            mazeRows = 15;

        // Maze Columns must be between 5 and 15;
        if (mazeColumns < 5)
            mazeColumns = 5;
        else if (mazeColumns > 15)
            mazeColumns = 15;

        maze = new GameObject[mazeRows * mazeColumns];
        BuildMaze(mazeRows, mazeColumns);
        AddMissingWaypoints(mazeRows);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void BuildMaze(int rows, int columns) {
        InstantiateBlocks(rows, columns);
        BuildPaths(rows, columns);
        CreateCoinsAndKey(rows, columns);
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
                Instantiate(waypointPrefab, position + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity, waypointsParent.transform);
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

    private void CreateCoinsAndKey(int rows, int columns) {
        List<int> listOfBlockIndex = GetIndexesForKeyAndCoins(numberOfItensInMaze, rows, columns);
        // First index is for key; others are for coins
        for (int i = 0; i < listOfBlockIndex.Count; i++) {
            if (i == 0) {
                Instantiate(keyPrefab, maze[listOfBlockIndex[i]].transform.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
            } else {
                Instantiate(coinPrefab, maze[listOfBlockIndex[i]].transform.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.identity, coinsParent.transform);
            }
        }
    }

    private List<int> GetIndexesForKeyAndCoins(int numberOfIndexes, int rows, int columns) {
        if (numberOfIndexes < 6) {
            numberOfIndexes = 6;
        } else if (numberOfIndexes > rows * columns) {
            numberOfIndexes = rows * columns;
        }

        List<int> indexes = new List<int>();

        for (int i = 0; i < numberOfIndexes; i++) {
            int index;
            do
            {
                index = Random.Range(0, rows * columns);
            } while (indexes.Contains(index));
            indexes.Add(index);
        }

        return indexes;
    }

    private void AddMissingWaypoints(int rows) {

        if (rows < 11) {
            Instantiate(waypointPrefab, new Vector3(0.0f, 3.0f, 48.0f), Quaternion.identity, waypointsParent.transform);
            Instantiate(waypointPrefab, new Vector3(0.0f, 3.0f, 57.0f), Quaternion.identity, waypointsParent.transform);
        }

        if (rows < 6) {
            Instantiate(waypointPrefab, new Vector3(0.0f, 3.0f, 65.0f), Quaternion.identity, waypointsParent.transform);
            Instantiate(waypointPrefab, new Vector3(0.0f, 3.0f, 74.0f), Quaternion.identity, waypointsParent.transform);
        }
    }
}
