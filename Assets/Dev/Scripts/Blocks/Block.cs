using System;
using System.Collections;
using Dev.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Scripts.Tiles
{
    public class Block : MonoBehaviour
    {
        #region Variables

        public RectTransform targetRectTransform;
        private Animator _animator;
        private Tile _tile;
        private BlockCreator _blockCreator;


        public Tile GetTile
        {
            get => _tile;
            set => _tile = value;
        }

        #endregion
        private void Start()
        {
            _blockCreator = FindObjectOfType<BlockCreator>();
            _animator = GetComponent<Animator>();

            GameEvents.OnLaneCompleted += LaneCompleted;
        }

        private void OnDestroy()
        {
            GameEvents.OnLaneCompleted -= LaneCompleted;
        }

        private void LaneCompleted(Tile tile)
        {
            if (tile==GetTile)
            {
                StartCoroutine(DoDestroy());
            }
        }
        
        private IEnumerator DoDestroy()
        {
            GetTile.LerpColor(1);
            _animator.Play("BlockDestroying");
            yield return new WaitForSeconds(1);
            _blockCreator.RemoveBlock(this);
            Destroy(this.gameObject);
        }


        #region Helper Methods

        public bool IsTargetNull() => targetRectTransform == null;
        public void SetBlockPosition()
        {
            _tile.AddBlock(this);
            
            transform.SetParent(targetRectTransform);
            var rect = GetComponent<RectTransform>();
            
            rect.sizeDelta = targetRectTransform.sizeDelta;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition=Vector2.zero;
        }

        #endregion

        #region Trigger Methods

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("BlankBlock"))
            {
                _tile = other.GetComponent<Tile>();
                targetRectTransform = other.GetComponent<RectTransform>();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BlankBlock"))
            {
                _tile = null;
                targetRectTransform = null;
            }
        }

        #endregion
        
    }
}