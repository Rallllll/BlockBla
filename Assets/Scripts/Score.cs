using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;

    private bool hasPlayedNewBest = false;

    // 1. BIẾN ĐỂ CHỨA THANH CHẠY (BAR FILL)
    public Image bestScoreBarFill;

    private int currentScore = 0;
    private int bestScore = 0;

    // ========================================================
    // QUẢN LÝ ĐỔI MÀU GẠCH THEO LƯỢT (HẾT 3 KHỐI LÀ ĐỔI)
    // ========================================================
    [Header("Block Colors List")]
    public Sprite[] blockColors;         // Danh sách chứa các hình ảnh màu gạch khác nhau

    // Biến static toàn cục giúp file Shape.cs có thể truy cập trực tiếp cực nhanh
    public static Sprite CurrentBlockColor;

    // THÊM: Biến ghi nhớ số thứ tự màu đang dùng
    private int currentColorIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // KHỞI TẠO MÀU BAN ĐẦU: Khi vừa vào game, dùng màu đầu tiên (phần tử số 0)
        if (blockColors != null && blockColors.Length > 0)
        {
            currentColorIndex = 0;
            CurrentBlockColor = blockColors[0];
        }

        UpdateScoreText();
        UpdateBestScoreBar();

        if (bestScoreText != null)
        {
            bestScoreText.text = bestScore.ToString();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;

        if (SoundEffect.Instance != null) SoundEffect.Instance.PlayAddScore();

        UpdateScoreText();
        UpdateBestScoreBar();
    }

    // ========================================================
    // THÊM HÀM MỚI: GỌI HÀM NÀY KHI HẾT 3 KHỐI GẠCH ĐỂ ĐỔI MÀU
    // ========================================================
    public void ChangeToNextColor()
    {
        if (blockColors == null || blockColors.Length == 0) return;

        // Chuyển sang số thứ tự tiếp theo, nếu vượt quá danh sách thì quay lại số 0 (Dùng phép chia dư %)
        currentColorIndex = (currentColorIndex + 1) % blockColors.Length;

        // Cập nhật bức ảnh màu gạch hiện tại
        CurrentBlockColor = blockColors[currentColorIndex];
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    private void UpdateBestScoreBar()
    {
        if (bestScoreBarFill != null)
        {
            if (bestScore > 0)
            {
                float fillPercentage = (float)currentScore / bestScore;
                bestScoreBarFill.fillAmount = Mathf.Clamp01(fillPercentage);
            }
            else
            {
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