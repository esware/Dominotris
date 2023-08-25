using System;
using System.Collections;
using System.Collections.Generic;
using Dev.Scripts;
using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using  UnityEngine.UI;

public class ModeChanger : MonoBehaviour,IPointerClickHandler
{
    
    public BlockType blockType;
    
    [Header("Mode Sprite")]
    [SerializeField] private Sprite modeOnSprite;
    [SerializeField] private Sprite modeOffSprite;
    
    private void Start()
    {
        SetButtonSprite(BoardManager.Instance.blockData.blockType);
        GameEvents.OnModeChanged += SetButtonSprite;
    }
    private void SetButtonSprite(BlockType b)
    {
        if (blockType == b)
        {
            GetComponent<Image>().sprite = modeOnSprite;
        }
        else
        {
            GetComponent<Image>().sprite = modeOffSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BoardManager.Instance.blockData.blockType != blockType)
        {
            BoardManager.Instance.blockData.blockType = blockType;
            GameEvents.OnModeChanged?.Invoke(blockType);
        }
    }
}
