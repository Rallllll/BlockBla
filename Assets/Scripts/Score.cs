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

    // ========================================================
    // THÊM: CÁC BIẾN QUẢN LÝ ĐỔI MÀU GẠCH (TẬP 20 & 21)
    // ========================================================
    [Header("Level Up Block Colors")]
    public Sprite[] blockColors;         // Danh sách chứa các hình ảnh màu gạch khác nhau
    public int scoreThreshold = 100;     // Cố định mốc điểm để đổi màu (Ví dụ: mỗi 100 điểm)

    // Biến static toàn cục giúp file Shape.cs có thể truy cập trực tiếp cực nhanh
    public static Sprite CurrentBlockColor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // KHỞI TẠO MÀU BAN ĐẦU: Khi vừa vào game, mặc định dùng màu đầu tiên (phần tử số 0)
        if (blockColors != null && blockColors.Length > 0)
        {
            CurrentBlockColor = blockColors[0];
        }

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

        // KIỂM TRA ĐỔI MÀU: Mỗi lần điểm tăng lên, tự động tính toán xem đã đến mốc đổi màu chưa
        CheckLevelUpColor();
    }

    // ========================================================
    // THÊM HÀM: TÍNH TOÁN CẤP ĐỘ MÀU DỰA TRÊN ĐIỂM SỐ
    // ========================================================
    private void CheckLevelUpColor()
    {
        if (blockColors == null || blockColors.Length == 0) return;

        // Chia lấy nguyên để xác định đang ở cấp độ màu mấy (Ví dụ: 250 điểm / 100 = Cấp 2)
        int currentLevel = currentScore / scoreThreshold;

        // Dùng phép chia lấy dư (%) để vòng lặp màu không bị lỗi vượt quá số lượng ảnh bạn có
        int colorIndex = currentLevel % blockColors.Length;

        // Cập nhật bức ảnh màu gạch hiện tại
        CurrentBlockColor = blockColors[colorIndex];
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