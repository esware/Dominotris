using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Dev.Scripts.Managers;
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

        [SerializeField] private BlockData blockData;
        [SerializeField] private Transform blockCarrier;
        [SerializeField] private AnimationCurve rotationCurve;
        [SerializeField] private List<Block> allBlocks;

        public List<Block> blocks;
        
        private bool _isRotating = false;

        #endregion


        private void Start()
        {
            SignUpEvents();
        }
        
        private void SignUpEvents()
        {
            GameEvents.OnBlockPleaced+=CreateNewBlock;
            GameEvents.OnNewGame += NewGame;
            GameEvents.OnModeChanged += ChangeMode;
            GameEvents.BlockPlaced += allBlocks.Add;
        }

        public void RemoveBlock(Block block)
        {
            allBlocks.Remove(block);
        }
        
        private void ChangeMode(BlockType type)
        {
            List<Block> blocksToRemove = new List<Block>();
            List<Block> blocksToAdd = new List<Block>();

            if (type== BlockType.Color)
            {
                foreach (var block in allBlocks)
                {
                    if (block.GetTile)
                    {
                        var colorPrefab = Random.Range(0, blockData.colorsPrefabs.Length - 1);
                        var newBlock = Instantiate(blockData.colorsPrefabs[colorPrefab], block.GetTile.transform);
                        
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
                        var numberPrefabs = Random.Range(0, blockData.colorsPrefabs.Length - 1);
                        var newBlock = Instantiate(blockData.numberPrefabs[numberPrefabs], block.GetTile.transform);

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
            var instance = BoardManager.Instance;
            instance.DestroyCompletedLanes();

            blocks.Clear();
            
            transform.rotation = Quaternion.identity;
            blockCarrier.transform.rotation =Quaternion.identity;
            
            
            if (blockData.blockType  == BlockType.Color)
            {
                for (int i = 0; i < 2; i++)
                {
                    var b = blockData.colorsPrefabs[Random.Range(0, blockData.colorsPrefabs.Length - 1)];

                    var  block = Instantiate(b, blockCarrier);
                    blocks.Add(block);
                } 
                
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    var b = blockData.numberPrefabs[Random.Range(0, blockData.numberPrefabs.Length - 1)];

                    var  block = Instantiate(b, blockCarrier);
                    blocks.Add(block);
                } 
            }
            
            if (instance.IsGameOver(blocks))
            {
                 Time.timeScale = 2f;
                 foreach (var b in allBlocks)
                 {
                     if (b!=null)
                     {
                         b.GetComponent<Collider>().isTrigger = false;
                         b.GetComponent<BoxCollider>().size = new Vector3(110, 110, 110);
                         var rigid =  b.GetComponent<Rigidbody>();
                         rigid.useGravity = true;
                     }
                 }
            }
           
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