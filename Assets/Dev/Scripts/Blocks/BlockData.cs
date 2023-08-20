using UnityEngine;
using NaughtyAttributes;

namespace Dev.Scripts.Tiles
{
    public enum BlockType
    {
        Number,
        Color,
    }
    
    [CreateAssetMenu(fileName = "BlockData",menuName = "EWGames/BlockData")]
    public class BlockData:ScriptableObject
    {
        public BlockType blockType;

        [ShowIf("blockType", BlockType.Color)] 
        [SerializeField] public Block[] colorsPrefabs;

        [ShowIf("blockType", BlockType.Number)] 
        [SerializeField] public Block[] numberPrefabs;
        
    }
}