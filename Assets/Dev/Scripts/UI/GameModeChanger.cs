using Dev.Scripts.Managers;
using Dev.Scripts.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Dev.Scripts
{
    public class GameModeChanger : MonoBehaviour,IPointerClickHandler
    {
        public GameMode gameMode;
        [Header("Mode Sprite")]
        [SerializeField] private Sprite modeOnSprite;
        [SerializeField] private Sprite modeOffSprite;
        
        private void Start()
        {
            SetButtonSprite(BoardManager.Instance.gameData.gameMode);
            GameEvents.OnGameModeChanged += SetButtonSprite;
        }
        private void SetButtonSprite(GameMode mode)
        {
            if (mode == gameMode)
            {
                GetComponent<Image>().sprite = modeOnSprite;
            }
            else
            {
                GetComponent<Image>().sprite = modeOffSprite;
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (BoardManager.Instance.gameData.gameMode != gameMode)
            {
                BoardManager.Instance.gameData.gameMode = gameMode;
                GameEvents.OnGameModeChanged?.Invoke(gameMode);
            }
        }
    }
}