using System.Collections.Generic;
using Dev.Scripts;
using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.UI;
public class BoardManager : MonoBehaviour
{
    #region Variables

    #region Singleton Properties
    
    private static BoardManager _instance;
    public static BoardManager Instance => _instance;
    
    #endregion

    #region Properties
    
    [Header("Board")]
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private RectTransform boardTransform;
    [Space, Header("BlockData")] public BlockData blockData;
    
    #endregion
    
    
    public Tile[,] Tiles;

    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CreateBoard();
    }
    
    public void CreateBoard()
    {
        ClearBoard();
        
        Tiles = new Tile[Helper.rowCount,Helper.columnCount];
        var rect = boardTransform.rect;
        var boardSize = new Vector2(rect.height-100, rect.width-100);
        var tileSize = new Vector2(boardSize.x / Helper.rowCount, boardSize.y / Helper.columnCount);
        var startedPosition = new Vector2(-boardSize.x/2+tileSize.x/2,-boardSize.y/2+tileSize.y/2);
        
        for (int i = 0; i < Helper.rowCount; i++)
        {
            for (int j = 0; j < Helper.columnCount; j++)
            {
                var tile = Instantiate(tilePrefab,boardTransform);
                var blockTransform = tile.GetComponent<RectTransform>();
                blockTransform.sizeDelta = tileSize;
                blockTransform.anchoredPosition = startedPosition+new Vector2(tileSize.x * i, tileSize.y * j);
                tile.x = i;
                tile.y = j;

                Tiles[i,j] = tile;
                tile.name = i +" "+ j + " Tile";
            }
        }
        
    }

    public void ClearBoard()
    {
        foreach (Transform tile in boardTransform)
        {
            Destroy(tile.gameObject);
        }
        
        Tiles = null;
    }
    public bool CanBlockPlace(List<Block> blocks)
    {
        var a = 0;
        
        foreach (var block in blocks)
        {
            if (Helper.IsAllNeighbourEmpty(block.GetTile))
            {
                a++;
            }
            else
            {
                if (Helper.IsSameBlockInAround(block.GetTile,block.name))
                {
                    return true;
                }
            }
        }

        if (a==2)
        {
            return true;
        }
        return false;

    }

    public bool IsGameOver(List<Block> blocks)
    {
        for (int i = 0; i < Helper.rowCount; i++)
        {
            for (int j = 0; j < Helper.columnCount; j++)
            {
                if (!Tiles[i, j].GetBlock)
                {
                    int[,] directions = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 } };

                    for (int k = 0; k < directions.GetLength(0); k++)
                    {
                        int newRow = i + directions[k, 0];
                        int newCol = j + directions[k, 1];

                        if (Helper.IsInsideMatrix(newRow, newCol) && !Tiles[newRow, newCol].GetBlock)
                        {
                            blocks[0].GetTile = Tiles[i, j];
                            blocks[1].GetTile = Tiles[newRow, newCol];
                            
                            for (int swap = 0; swap < 2; swap++)
                            {
                                Tile temp = blocks[0].GetTile;
                                blocks[0].GetTile = blocks[1].GetTile;
                                blocks[1].GetTile = temp;
           
                                if (CanBlockPlace(blocks))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }

        GameEvents.OnGameOver?.Invoke();
        return true;
    }
    
    public void DestroyCompletedLanes()
    {
        for (int i = 0; i < Helper.rowCount; i++)
        {
            bool isRowFilled = true;
            bool isColumnFilled = true;
            
            for (int j = 0; j < Helper.columnCount; j++)
            {
                if (!Helper.GetTile(i, j).GetBlock)
                {
                    isRowFilled = false;
                }

                if (!Helper.GetTile(j, i).GetBlock)
                {
                    isColumnFilled = false;
                }
            }
            
            if (isRowFilled)
            {
                Helper.DestroyRow(i);
            }

            if (isColumnFilled)
            {
                Helper.DestroyCol(i);
            }
        }
    }
    

}
