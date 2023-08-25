using Dev.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dev.Scripts
{
    public class ButtonHandler : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private GameObject nextPanel;
        [SerializeField] private GameObject currentPanel;

        private void OnClick()
        {
            currentPanel.SetActive(false);
            nextPanel.SetActive(true);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }
    }
}