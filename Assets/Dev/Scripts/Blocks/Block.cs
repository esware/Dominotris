using UnityEngine;

namespace Dev.Scripts.Tiles
{
    public class Block : MonoBehaviour
    {
        private RectTransform targetRectTransform;
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("BlankBlock"))
            {
                targetRectTransform = other.GetComponent<RectTransform>();
            }
        }


        public void GoPlace()
        {
            if (!targetRectTransform)
            {
                transform.SetParent(targetRectTransform);
                var rect = GetComponent<RectTransform>();
            
                rect.sizeDelta = targetRectTransform.sizeDelta;
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition=Vector2.zero;
            }
        }
    }
}