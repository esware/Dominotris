using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Dev.Scripts
{
    public class ContinueButton : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private GameObject nextPanel;
        [SerializeField] private GameObject currentPanel;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (BoardManager.Instance.Tiles!=null)
            {
                currentPanel.SetActive(false);
                nextPanel.SetActive(true);
            }
        }
    }
}