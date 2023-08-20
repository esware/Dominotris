using System.Collections;
using System.Collections.Generic;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Dev.Scripts
{
    public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _originalParent;
        private Transform _canvas;

        public List<Block> _blocks;
        
        private void Start()
        {
            _canvas = GameObject.Find("Canvas").transform;
            
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            transform.SetParent(_canvas);

            _blocks = new List<Block>();
            for (int i = 0; i < 2; i++)
            {
                _blocks.Add(transform.GetChild(i).GetComponent<Block>());
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
            foreach (var block in _blocks)
            {
                block.GoPlace();
            }
        }
    }
}