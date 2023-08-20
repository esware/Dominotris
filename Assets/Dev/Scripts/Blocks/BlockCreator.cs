using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dev.Scripts.Tiles
{
    public class BlockCreator:MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private BlockData blockData;
        [SerializeField] private Transform blockCarrier;
        [SerializeField] private AnimationCurve rotationCurve;


        private bool _isRotating = false;
        private void Start()
        {
            InitData();
        }

        private void InitData()
        {
            if (blockData.blockType  == BlockType.Color)
            {
                for (int i = 0; i < 2; i++)
                {
                    var b = blockData.colorsPrefabs[Random.Range(0, blockData.colorsPrefabs.Length - 1)];

                    Instantiate(b, blockCarrier);

                }
            }
            else
            {
                
            }
        }


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
    }
}