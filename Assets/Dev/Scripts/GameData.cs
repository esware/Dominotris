using UnityEngine;
using NaughtyAttributes;

namespace Dev.Scripts.Tiles
{
    public enum BlockType
    {
        Number,
        Color,
    }
    
    public enum GameMode{ Normal,Hard}
    
    [CreateAssetMenu(fileName = "GameData",menuName = "EWGames/GameData")]
    public class GameData:ScriptableObject
    {
        public BlockType blockType;

        [ShowIf("blockType", BlockType.Color)] 
        [SerializeField] public Block[] colorsPrefabs;

        [ShowIf("blockType", BlockType.Number)] 
        [SerializeField] public Block[] numberPrefabs;

        [SerializeField] public GameMode gameMode;

    }
}