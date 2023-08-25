using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dev.Scripts
{
    public class PlayerControl:MonoBehaviour
    {
        public int playerScore;

        public void AddScore(int score)
        {
            playerScore += score;
        }

        public void GetHighScore()
        {
            var  score = PlayerPrefs.GetInt("PlayerHighScore");

            if (score < playerScore)
            {
                Debug.Log("High Score" +score);
            }
        }
    }
}