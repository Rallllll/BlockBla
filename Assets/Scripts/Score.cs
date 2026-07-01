using UnityEngine;
using TMPro;
using UnityEngine.UI; // THÊM THƯ VIỆN NÀY ĐỂ ĐIỀU KHIỂN IMAGE

public class Score : MonoBehaviour
{
    public static Score Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    // 1. BIẾN ĐỂ CHỨA THANH CHẠY (BAR FILL)
    public Image bestScoreBarFill;

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

        // Cập nhật thanh Bar ngay lúc mới mở game
        UpdateBestScoreBar();

        if (bestScoreText != null)
        {
            bestScoreText.text = bestScore.ToString();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreText();

        // Cập nhật thanh Bar mỗi khi ăn điểm
        UpdateBestScoreBar();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    // --- HÀM MỚI: TÍNH TOÁN % VÀ ĐỔ ĐẦY THANH BAR ---
    private void UpdateBestScoreBar()
    {
        if (bestScoreBarFill != null)
        {
            if (bestScore > 0)
            {
                // Công thức: % = Điểm hiện tại / Điểm kỷ lục
                float fillPercentage = (float)currentScore / bestScore;

                // Mathf.Clamp01 giúp giới hạn giá trị từ 0 đến 1 (để thanh không bị trào ra ngoài khi phá kỷ lục)
                bestScoreBarFill.fillAmount = Mathf.Clamp01(fillPercentage);
            }
            else
            {
                // Nếu chưa có kỷ lục nào (lần đầu chơi), cho thanh = 0
                bestScoreBarFill.fillAmount = 0f;
            }
        }
    }

    public bool CheckAndSaveBestScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();

            if (bestScoreText != null)
            {
                bestScoreText.text = bestScore.ToString();
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