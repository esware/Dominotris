using System;
using System.Collections;
using Dev.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Dev.Scripts.Tiles
{
    public class Tile:MonoBehaviour
    {
        public Block _block;
        public Block GetBlock
        {
            get => _block;
            set => _block = value;
        }

        public int x, y;

        public void AddBlock(Block block)
        {
            _block = block;
        }
        
        public bool AlreadyHaveBlock() => _block != null;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Block"))
            {
                LerpColor(0.5f);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Block"))
            {
                LerpColor(1f);
            }
        }

        public void LerpColor(float value)
        {
            var tileImage = GetComponent<Image>();
            
            tileImage.DOColor(Color.white *value, .5f);
        }
        

    }
}