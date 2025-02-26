using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class MazeGenerate : MonoBehaviour
{
    /*
     * Code based on Apolok99's Unity Maze Generator: https://github.com/Apolok99/Maze-Random-Generator-Unity
     * Based on Prim's algorithm.
     */
    
    private const bool Wall = true;
    private const bool Passage = false;

    public UnityEvent OnGameStart;

    public GameObject prefabWall;
    
    public int mazeWidth;
    public int mazeHeight;

    private int startCellY, startCellX;
    
    public Vector2 startPoint;
    
    // Two dimensional array for grid and structure
    private bool[,] _mazeGrid;

    private GameObject[,] _mazeStructure;

    void Start()
    {
        startCellX = Random.Range(3, mazeWidth - 3);
        startCellY = Random.Range(3, mazeHeight - 3);
        startPoint = new Vector2(startCellX, startCellY);
        
        OnGameStart?.Invoke();
        
        GenerateMaze();
    }
    

    private void GenerateMaze()
    {
        _mazeGrid = new bool[mazeWidth, mazeHeight];
        _mazeStructure = new GameObject[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                _mazeGrid[x, y] = Wall;
                _mazeStructure[x, y] = Instantiate(prefabWall, new Vector2(x * 1f, y * 1f), Quaternion.identity,
                    GetComponent<Transform>());
            }
        }
        
        // Select a random cell to be the starting point
        
        _mazeGrid[startCellX, startCellY] = Passage;

        HashSet<(int, int)> frontierCells = GetNeighborCells(startCellX, startCellY, true);
        
        // While there's frontier cells, continue creating new passages
        while (frontierCells.Any())
        {
            // Select a random frontier cell and change it into a passage
            int randomIndex = Random.Range(0, frontierCells.Count);
            (int, int) randomFrontierCell = frontierCells.ElementAt(randomIndex);
            int randomFrontierCellX = randomFrontierCell.Item1;
            int randomFrontierCellY = randomFrontierCell.Item2;
            _mazeGrid[randomFrontierCellX, randomFrontierCellY] = Passage;

            // Obtain the list of passage cells which can be connected to the selected frontier cell
            HashSet<(int, int)> candidateCells = GetNeighborCells(randomFrontierCellX, randomFrontierCellY, false);
            if (candidateCells.Any())
            {
                // Select a random passage to connect with the frontier cell
                int randomIndexCandidate = Random.Range(0, candidateCells.Count);
                (int, int) randomCellConnection = candidateCells.ElementAt(randomIndexCandidate);
                int randomCellConnectionX = randomCellConnection.Item1;
                int randomCellConnectionY = randomCellConnection.Item2;
                
                // Calculate which cell is inbetween the frontier cell and the passage
                (int, int) cellBetween;
                if (randomFrontierCellX < randomCellConnectionX)
                    cellBetween = (randomFrontierCellX + 1, randomFrontierCellY);
                else if (randomFrontierCellX > randomCellConnectionX)
                    cellBetween = (randomFrontierCellX - 1, randomFrontierCellY);
                else
                {
                    if (randomFrontierCellY < randomCellConnectionY)
                        cellBetween = (randomFrontierCellX, randomFrontierCellY + 1);
                    else
                        cellBetween = (randomFrontierCellX, randomFrontierCellY - 1);
                }
                
                // Make the cell in between a passage to connect the frontier and passage cell
                _mazeGrid[cellBetween.Item1, cellBetween.Item2] = Passage;
            }
            // remove the frontier cell that has been converted to a passage
            frontierCells.Remove(randomFrontierCell);
            
            frontierCells.UnionWith(GetNeighborCells(randomFrontierCellX, randomFrontierCellY, true));
        }
        
        // Deactivate the GameObjects of the walls that are a passage
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (_mazeGrid[x, y] == Passage)
                {
                    _mazeStructure[x,y].SetActive(false);
                }
            }
        }
    }

    private HashSet<(int, int)> GetNeighborCells(int x, int y, bool checkFrontier)
    {
        HashSet<(int, int)> newNeighbourCells = new HashSet<(int, int)>();
        
        // Check if the cell can have a neighbour cell
        if (x > 2)
        {
            (int, int) cellToCheck = (x - 2, y);

            if (checkFrontier
                    ? _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Wall
                    : _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Passage)
            {
                newNeighbourCells.Add(cellToCheck);
            }
        }
        if (x < mazeWidth - 3)
        {
            (int, int) cellToCheck = (x + 2, y);
            if (checkFrontier
                    ? _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Wall
                    : _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Passage)
            {
                newNeighbourCells.Add(cellToCheck);
            }
        }

        if (y > 2)
        {
            (int, int) cellToCheck = (x, y - 2);
            if (checkFrontier
                    ? _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Wall
                    : _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Passage)
            {
                newNeighbourCells.Add(cellToCheck);
            }
        }

        if (y < mazeHeight - 3)
        {
            (int, int) cellToCheck = (x, y + 2);
            if (checkFrontier
                    ? _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Wall
                    : _mazeGrid[cellToCheck.Item1, cellToCheck.Item2] == Passage)
            {
                newNeighbourCells.Add(cellToCheck);
            }
        }
        return newNeighbourCells;
    }
}
