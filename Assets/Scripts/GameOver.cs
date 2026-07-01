using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Bắt buộc phải có thư viện này để điều khiển chữ TextMeshPro

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    public GameObject gameOverPopup;     // Cái khung tổng (Nền đen + Nút bấm)

    [Header("UI Groups")]
    public GameObject normalGameOverGroup; // Kéo Object "GameOver" từ Hierarchy vào đây
    public GameObject newBestScoreGroup;   // Kéo Object "BestScore" từ Hierarchy vào đây

    [Header("Score Display")]
    public TextMeshProUGUI popupScoreText; // Tạo 1 cái Text nằm trên ô ScoreBox rồi kéo vào đây để hiện điểm ván này

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPopup != null)
        {
            // 1. Hiện cái bảng to lên trước
            gameOverPopup.SetActive(true);

            // 2. Lấy điểm số hiện tại từ script Score và hiển thị lên bảng Popup
            if (Score.Instance != null && popupScoreText != null)
            {
                int finalScore = Score.Instance.GetCurrentScore();
                popupScoreText.text = finalScore.ToString();
            }

            // 3. Hỏi Score xem ván này có phá kỷ lục không?
            bool isNewRecord = Score.Instance.CheckAndSaveBestScore();

            // 4. Tắt/Bật các nhóm chữ tuyệt đối (Không bao giờ bị hiện cả hai)
            if (isNewRecord)
            {
                // Nếu phá kỷ lục: Bật chữ New Best Score, Tắt chữ Game Over thường
                if (newBestScoreGroup != null) newBestScoreGroup.SetActive(true);
                if (normalGameOverGroup != null) normalGameOverGroup.SetActive(false);
            }
            else
            {
                // Nếu thua bình thường: Tắt chữ New Best Score, Bật chữ Game Over thường
                if (newBestScoreGroup != null) newBestScoreGroup.SetActive(false);
                if (normalGameOverGroup != null) normalGameOverGroup.SetActive(true);
            }
        }
    }

    // Hàm gắn vào nút Try Again
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hàm gắn vào nút Exit
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}