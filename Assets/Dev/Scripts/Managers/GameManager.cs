using System;
using Dev.Scripts.Tiles;
using UnityEngine;

namespace Dev.Scripts.Managers
{
    public struct GameEvents
    {
        public static Action<Tile> OnLaneCompleted;
        public static Action OnBlockPleaced;
        public static Action<Block> BlockPlaced;
        public static Action OnGameOver;
        public static Action<BlockType> OnBlockTypeChanged;
        public static Action OnNewGame;
        public static Action<GameMode> OnGameModeChanged;


        public static void ResetEvents()
        {
            OnBlockPleaced = null;
            OnLaneCompleted = null;
            OnBlockTypeChanged = null;
            OnNewGame = null;
            BlockPlaced = null;
            OnGameModeChanged = null;
        }
    
    }
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            GameEvents.ResetEvents();
        }


    }
}