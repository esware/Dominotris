using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Dev.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Dev.Scripts.Tiles
{
    public class BlockCreator:MonoBehaviour,IPointerClickHandler
    {
        #region Variables

        [SerializeField] private GameData gameData;
        [SerializeField] private Transform blockCarrier;
        [SerializeField] private AnimationCurve rotationCurve;
        [SerializeField] private List<Block> allBlocks;

        public List<Block> blocks;
        
        private bool _isRotating = false;
        private Block _previousSelectedBlock;

        #endregion


        private void Start()
        {
            SignUpEvents();
        }
        
        private void SignUpEvents()
        {
            GameEvents.OnBlockPleaced+=CreateNewBlock;
            GameEvents.OnNewGame += NewGame;
            GameEvents.OnBlockTypeChanged += ChangeBlockType;
            GameEvents.BlockPlaced += allBlocks.Add;
        }

        public void RemoveBlock(Block block)
        {
            allBlocks.Remove(block);
        }
        
        private void ChangeBlockType(BlockType type)
        {
            List<Block> blocksToRemove = new List<Block>();
            List<Block> blocksToAdd = new List<Block>();

            if (type== BlockType.Color)
            {
                foreach (var block in allBlocks)
                {
                    if (block.GetTile)
                    {
                        var colorPrefab = Random.Range(0, gameData.colorsPrefabs.Length - 1);
                        var newBlock = Instantiate(gameData.colorsPrefabs[colorPrefab], block.GetTile.transform);
                        
                        newBlock.GetTile = block.GetTile;
                        newBlock.targetRectTransform = block.targetRectTransform;
                        newBlock.SetBlockPosition();
                        
                        blocksToRemove.Add(block);
                        blocksToAdd.Add(newBlock);
                    }
                }

                foreach (var blockToRemove in blocksToRemove)
                {
                    allBlocks.Remove(blockToRemove);
                    Destroy(blockToRemove.gameObject);
                }
                foreach (var block in blocksToAdd)
                {
                    allBlocks.Add(block);
                }

                foreach (Transform block in blockCarrier)
                {
                    Destroy(block.gameObject);
                }

            }
            else
            {
                foreach (var block in allBlocks)
                {
                    if (block.GetTile)
                    {
                        var numberPrefabs = Random.Range(0, gameData.colorsPrefabs.Length - 1);
                        var newBlock = Instantiate(gameData.numberPrefabs[numberPrefabs], block.GetTile.transform);

                        newBlock.GetTile = block.GetTile;
                        newBlock.targetRectTransform = block.targetRectTransform;
                        newBlock.SetBlockPosition();
                        
                        blocksToRemove.Add(block);
                        blocksToAdd.Add(newBlock);
                    }
                }

                foreach (var blockToRemove in blocksToRemove)
                {
                    allBlocks.Remove(blockToRemove);
                    Destroy(blockToRemove.gameObject);
                }
                foreach (var block in blocksToAdd)
                {
                    allBlocks.Add(block);
                }
                foreach (Transform block in blockCarrier)
                {
                    Destroy(block.gameObject);
                }
            }
            
            CreateNewBlock();

        }

        private void NewGame()
        {
            foreach (Transform block in blockCarrier)
            {
                Destroy(block.gameObject);
            }
            
            allBlocks.Clear();
            CreateNewBlock();
        }

        private void OnEnable()
        {
            CreateNewBlock();
        }

        private void OnDisable()
        {
            foreach (Transform block in blockCarrier)
            {
                Destroy(block.gameObject);
            }
            blocks.Clear();
        }

        private void CreateNewBlock()
        {
            transform.rotation = Quaternion.identity;
            blockCarrier.transform.rotation =Quaternion.identity;
            
            var instance = BoardManager.Instance;
            instance.DestroyCompletedLanes();

            blocks.Clear();

            CreateBlockForGameMode(gameData.gameMode);
            
            if (instance.IsGameOver(blocks))
                StartCoroutine(DoGameOver());

        }

        private void CreateBlockForGameMode(GameMode gameMode)
        {
            if (gameMode == GameMode.Hard)
            {
                if (allBlocks.Count > 0)
                {
                    CreatePreferredBlocks();
                }
                else
                {
                    CreateRandomBlocks();
                }
            }
            else
            {
                CreateRandomBlocks();
            }
        }
        
        private void CreateRandomBlocks()
        {
            for (int i = 0; i < 2; i++)
            {
                var b = gameData.blockType == BlockType.Color ?
                    gameData.colorsPrefabs[Random.Range(0, gameData.colorsPrefabs.Length)] :
                    gameData.numberPrefabs[Random.Range(0, gameData.numberPrefabs.Length)];

                var block = Instantiate(b, blockCarrier);
                blocks.Add(block);
            }
        }
        
        private void CreatePreferredBlocks()
        {
            Dictionary<Block, int> blockCounts = Helper.CountBlocksInScene(allBlocks);

            for (int i = 0; i < 2; i++)
            {
                Block selectedBlock;
                
                do
                {
                    selectedBlock = gameData.blockType == BlockType.Color 
                        ? gameData.colorsPrefabs[Helper.ChoosePreferredBlockIndex(blockCounts,gameData)]
                        : gameData.numberPrefabs[Helper.ChoosePreferredBlockIndex(blockCounts,gameData)];
                }
                while (selectedBlock == _previousSelectedBlock);

                _previousSelectedBlock = selectedBlock;

                var block = Instantiate(selectedBlock, blockCarrier);
                blocks.Add(block);
            }
        }
        
        private IEnumerator DoGameOver()
        {
            foreach (var b in allBlocks)
            {
                if (b!=null)
                {
                    b.GetComponent<Collider>().isTrigger = false;
                    b.GetComponent<BoxCollider>().size = new Vector3(110, 110, 110);
                    var rigid =  b.GetComponent<Rigidbody>();
                    rigid.useGravity = true;
                }

                yield return null;
            }

            yield return new WaitForSecondsRealtime(4f);
            
            BoardManager.Instance.CreateBoard();
            NewGame();
        }


        #region Rotateting Methods

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isRotating)
            {
                StartCoroutine(DoRotate());
            }
        }

        private IEnumerator DoRotate()
        {
            _isRotating = true;
            var duration = .5f;
            var t = 0f;
            
            Quaternion initialRot = transform.rotation;
            Quaternion targetRot = Quaternion.Euler(0, 0, initialRot.eulerAngles.z - 90);

            while (t<duration)
            {
                float normalizedTime = t / duration;
                float curveValue = rotationCurve.Evaluate(normalizedTime);
                
                var currentRot = Quaternion.Slerp(initialRot, targetRot, curveValue);
                transform.rotation = currentRot;
                t += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRot;
            _isRotating = false;
        }

        #endregion
    }
}