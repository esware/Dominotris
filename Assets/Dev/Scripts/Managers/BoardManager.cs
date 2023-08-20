using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    #region Properties
    [Header("Board")]
    [SerializeField] private BlankBlock blankBlockPrefab;
    [SerializeField] private RectTransform boardTransform;
    [SerializeField] private int rowCount;
    [SerializeField] private int columnCount;
    
    

    #endregion

    private void Awake()
    {
        CreateBoard();
    }


    private void CreateBoard()
    {
        var rect = boardTransform.rect;
        var boardSize = new Vector2(rect.height-100, rect.width-100);
        var blockSize = new Vector2(boardSize.x / rowCount, boardSize.y / columnCount);
        var startedPosition = new Vector2(-boardSize.x/2+blockSize.x/2,-boardSize.y/2+blockSize.y/2);
        
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var block = Instantiate(blankBlockPrefab,boardTransform);
                var blockTransform = block.GetComponent<RectTransform>();
                blockTransform.sizeDelta = blockSize;
                blockTransform.anchoredPosition = startedPosition+new Vector2(blockSize.x * i, blockSize.y * j);

               // block.position = startedPosition + new Vector2(blockSize.x * i, blockSize.y * j);
                //block.size = blockSize;
                block.name = i + j + "block";
            }
        }
    }


}
