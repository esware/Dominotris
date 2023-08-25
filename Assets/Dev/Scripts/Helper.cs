using System.Collections.Generic;
using System.Linq;
using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Scripts
{
    public class Helper
    {
            public static readonly int rowCount=5;
            public static readonly int columnCount=5;

            public static bool IsInsideMatrix(int row, int column)
            {
                return row >= 0 && row < rowCount && column >= 0 && column < columnCount;
            }
            public static Tile GetTile(int row, int column)
            {
                return BoardManager.Instance.Tiles[row, column];
            }
            public static List<Tile> GetNeighbourTiles(int row, int column)
            {
                List<Tile> neighbors = new List<Tile>();
        
                if (IsInsideMatrix(row - 1, column)) neighbors.Add(GetTile(row - 1, column)); 
                if (IsInsideMatrix(row + 1, column)) neighbors.Add(GetTile(row + 1, column)); 
                if (IsInsideMatrix(row, column - 1)) neighbors.Add(GetTile(row, column - 1)); 
                if (IsInsideMatrix(row, column + 1)) neighbors.Add(GetTile(row, column + 1)); 
        
                return neighbors;
            }
            
            public static void DestroyCol(int index)
            {
                for (int j = 0; j < 5; j++)
                {
                    GameEvents.OnLaneCompleted?.Invoke(GetTile(j,index));
                }
            }
        
            public static void DestroyRow(int index)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameEvents.OnLaneCompleted?.Invoke(GetTile(index,i));
                }
            }
        
            public static bool IsAllNeighbourEmpty(Tile tile)
            {
                var x = tile.x;
                var y = tile.y;
                
                foreach (var neighborBlocks in GetNeighbourTiles(x, y))
                {
                    if (neighborBlocks.GetComponent<Tile>().AlreadyHaveBlock())
                    {
                        return false;
                    }
                }
        
                return true;
            }
        
            public static bool IsSameBlockInAround(Tile tile,string objectName)
            {
                var x = tile.x;
                var y = tile.y;
                
                foreach (var neighborBlocks in GetNeighbourTiles(x,y))
                {
                    if (neighborBlocks.GetBlock!=null)
                    {
                        if (neighborBlocks.GetBlock.name == objectName)
                        {
                            return true;
                        }
                    }
                }
        
                return false;
            }
            
                    public static  Dictionary<Block, int> CountBlocksInScene(List<Block> allBlocks)
                    {
                        Dictionary<Block, int> blockCounts = new Dictionary<Block, int>();
            
                        foreach (var blockPrefab in allBlocks)
                        {
                            int count = allBlocks.Count(b => b.name == blockPrefab.name);
                            blockCounts[blockPrefab] = count;
                        }
            
                        return blockCounts;
                    }
                    
                    public static  int ChoosePreferredBlockIndex(Dictionary<Block, int> blockCounts,GameData gameData)
                    {
                        float totalBlockCount = blockCounts.Count;
                        float[] blockProbabilities = new float[blockCounts.Count];
            
                        int index = 0;
                        foreach (var kvp in blockCounts)
                        {
                            float probability = 1.0f - (kvp.Value / totalBlockCount);
                            blockProbabilities[index] = probability;
                            index++;
                        }
            
                        int maxIndex = Mathf.Min(blockProbabilities.Length, gameData.colorsPrefabs.Length);
                        int selectedBlockIndex = RandomIndexWithProbabilities(blockProbabilities);
                        
                        selectedBlockIndex = Mathf.Clamp(selectedBlockIndex, 0, maxIndex - 1);
                        return selectedBlockIndex;
                    }
                    
                    private static int RandomIndexWithProbabilities(float[] probabilities)
                    {
                        float total = probabilities.Sum();
                        float randomValue = Random.Range(0f, total);
                        float cumulativeProb = 0f;
            
                        for (int i = 0; i < probabilities.Length; i++)
                        {
                            cumulativeProb += probabilities[i];
                            if (randomValue <= cumulativeProb)
                            {
                                return i;
                            }
                        }
            
                        return probabilities.Length - 1; 
                    }
            
                    

    }
}