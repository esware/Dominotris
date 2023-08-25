using System;
using System.Collections;
using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dev.Scripts
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Properties

        [SerializeField]  private RectTransform originalParent;
        [SerializeField] private AnimationCurve positionCurve;

        #endregion
        
        #region Private Variables
        
        private Transform _canvas;
        private BlockCreator _blockCreator;

        #endregion


        private void Start()
        {
            _canvas = GameObject.Find("Canvas").transform;
            _blockCreator = GetComponentInParent<BlockCreator>();
        }


        private bool IsTileNotNull()
        {
            foreach (var block in _blockCreator.blocks)
            {
                if (block.IsTargetNull())
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsTileHaveBlock()
        {
            foreach (var block in _blockCreator.blocks)
            {
                if (block.GetTile.GetBlock!=null)
                {
                    return true;
                }
            }

            return false;
        }
        
        
        #region Draging Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_canvas);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsTileNotNull())
            {
                if (!IsTileHaveBlock())
                {
                    if (BoardManager.Instance.CanBlockPlace(_blockCreator.blocks))
                    {
                        foreach (var i in _blockCreator.blocks)
                        {
                            i.SetBlockPosition();
                            GameEvents.BlockPlaced?.Invoke(i);
                        }
                        GameEvents.OnBlockPleaced?.Invoke();
                    }
                   
                }
            }

            transform.SetParent(originalParent);
            StartCoroutine(ResetPosition());
            
        }

        #endregion
        
        private IEnumerator ResetPosition()
        {
            var duration = .5f;
            var t = 0f;
            
            Vector2 initalPos = transform.GetComponent<RectTransform>().anchoredPosition;
            Vector2 targetPos = Vector2.zero;

            while (t<duration)
            {
                float normalizedTime = t / duration;
                float curveValue = positionCurve.Evaluate(normalizedTime);
                
                var currentPos = Vector2.Lerp(initalPos, targetPos, curveValue);
                transform.GetComponent<RectTransform>().anchoredPosition = currentPos;
                t += Time.deltaTime;
                yield return null;
            }
            transform.GetComponent<RectTransform>().anchoredPosition = targetPos;
        }
    }
}