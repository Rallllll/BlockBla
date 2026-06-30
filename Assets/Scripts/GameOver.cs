using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    public GameObject gameOverPopup; 

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
            gameOverPopup.SetActive(true);

            // BẢO SCORE MANAGER KIỂM TRA ĐIỂM
            bool isNewRecord = Score.Instance.CheckAndSaveBestScore();

            if (isNewRecord)
            {
                Debug.Log("CHÚC MỪNG! BẠN VỪA LẬP KỶ LỤC MỚI!");
                // (Tập sau chúng ta sẽ đổi chữ "Game Over" trên màn hình thành chữ "Kỷ Lục Mới" ở đây)
            }
            else
            {
                Debug.Log("GÀ QUÁ, CHƯA PHÁ ĐƯỢC KỶ LỤC!");
            }
        }
    }

    // Hàm gắn vào nút Try Again
    public void RestartGame()
    {
        // Load lại đúng cái Scene hiện tại (Ví dụ Scene của bạn tên là "Game")
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hàm gắn vào nút Exit
    public void ExitToMenu()
    {
        // Đổi "MainMenu" thành tên cái Scene Menu của bạn (nếu có)
        SceneManager.LoadScene("MainMenu");
    }
}