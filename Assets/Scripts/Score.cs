using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public TextMeshProUGUI scoreText;

    // 1. THÊM BIẾN NÀY ĐỂ CHỨA CHỮ BEST SCORE
    public TextMeshProUGUI bestScoreText;

    private int currentScore = 0;
    private int bestScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreText();

        // 2. GÁN SỐ VÀO CHỮ BEST SCORE KHI VỪA MỞ GAME
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best: " + bestScore.ToString();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    public bool CheckAndSaveBestScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();

            // Tự động cập nhật lại số trên màn hình ngay lúc phá kỷ lục
            if (bestScoreText != null)
            {
                bestScoreText.text = "Best: " + bestScore.ToString();
            }

            return true;
        }
        return false;
    }
    public int GetCurrentScore()
    {
        return currentScore;
    }
}