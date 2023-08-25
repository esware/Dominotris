using Dev.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dev.Scripts
{
    public class NewGameButton : MonoBehaviour,IPointerClickHandler
    {
        [SerializeField] private GameObject nextPanel;
        [SerializeField] private GameObject currentPanel;
        public void OnPointerClick(PointerEventData eventData)
        {
            currentPanel.SetActive(false);
            nextPanel.SetActive(true);
            GameEvents.OnNewGame?.Invoke();
        }
    }
}