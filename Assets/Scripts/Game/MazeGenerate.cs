using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

public class MazeGenerate : MonoBehaviour
{
    /*
     * Code based on Apolok99's Unity Maze Generator: https://github.com/Apolok99/Maze-Random-Generator-Unity
     * Based on Prim's algorithm.
     */
    
    private const bool Wall = true;
    private const bool Passage = false;
    private const int minigameSpawnChance = 4;
    
    // minigame node prefab
    [SerializeField] private GameObject minigamePrefab;

    public UnityEvent OnGameStart;
    
    // Parent object to create minigame nodes under
    [SerializeField] private Transform minigameParent;

    // Maze tilemap
    public Tile wallTile;
    public Tilemap mazeTilemap;
    
    // Maze parameters
    public int mazeWidth;
    public int mazeHeight;
    
    // Starting point of maze
    public Vector2 startPoint;
    private int startCellY, startCellX;
    
    // Two dimensional array for grid and structure
    private bool[,] _mazeGrid;

    // List of a tuple to hold coordinates of possible minigame cells
    [SerializeField] private List<(int x, int y)> minigameCells = new List<(int, int)>();

    void Start()
    {
        // Change wall color to green
        wallTile.color = Color.green;
        
        // Choose a random starting point for the maze 
        startCellX = Random.Range(3, mazeWidth - 3);
        startCellY = Random.Range(3, mazeHeight - 3);
        startPoint = new Vector2(startCellX, startCellY);
        
        // Tell the other scripts to set player and camera position to starting point
        OnGameStart?.Invoke();
        
        // Generate the maze
        GenerateMaze();
        
        // Find minigame cells
        for (int row = 1; row < mazeHeight - 1; row++)
        {
            // Search this row for any dead ends
            FindPathEnd(row, _mazeGrid);
        }
        // For each cell in the list, there is a 1/3 chance to spawn a node there
        foreach(var cells in minigameCells)
        {
            int chance = Random.Range(0, minigameSpawnChance);
            if (chance == minigameSpawnChance - 1)
                Instantiate(minigamePrefab, new Vector2(cells.Item1, cells.Item2), Quaternion.identity, minigameParent);
        }
    }
    
    private void GenerateMaze()
    {
        _mazeGrid = new bool[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                _mazeGrid[x, y] = Wall;
                mazeTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
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
                    // Check for all sides, if there's only one opening, then
                    mazeTilemap.SetTile(new Vector3Int(x, y, 0), null);
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

    /// <summary>
    /// Goes through a row of tiles and add any dead-end cells to a list of possible
    /// minigame nodes.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="array"></param>
    private void FindPathEnd(int row, bool[,] array)
    {
        for (int col = 1; col < mazeWidth - 1; col++)
        {
            int passageCount = 0;
            
            if (array[row, col] == Wall)
                continue;

            // Look at the adjacent cells and count how many are passage tiles (empty tiles)
            if(array[row, col + 1] == Passage)
                passageCount++;
            if(array[row + 1, col] == Passage)
                passageCount++;
            if(array[row, col - 1] == Passage)
                passageCount++;
            if(array[row - 1, col] == Passage)
                passageCount++;

            // If there is more than one passage, then don't add a minigame node
            if (passageCount > 1)
                continue;
            
            // otherwise, add the coordinates of the tile to the minigame array
            minigameCells.Add((row, col));
        }
    }
}
