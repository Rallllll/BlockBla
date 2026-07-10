using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonManager : MonoBehaviour
{
    [Header("--- CÀI ĐẶT UI ---")]
    public GameObject backPopup; // Kéo cái bảng Pop-up bồ đã làm sẵn vào đây
    public string menuSceneName = "MainMenu"; // Chỉnh lại tên Scene Menu của bồ nếu cần

    private void Start()
    {
        // 1. Vừa vào game là tự động ẩn cái Pop-up đi
        if (backPopup != null)
        {
            backPopup.SetActive(false);
        }
    }

    // ===============================================================
    // 2. GẮN VÀO NÚT BACK Ở NGOÀI MÀN HÌNH CHƠI
    // ===============================================================
    public void OnClick_MainBackButton()
    {
        if (Grid.Instance == null) return;

        // Hỏi lưới xem có đang trống không (Hàm IsGridEmpty mình đã chèn vào Grid.cs lúc nãy)
        if (Grid.Instance.IsGridEmpty())
        {
            // Nếu trống (chưa chơi) -> Về thẳng Menu luôn
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            // Nếu đang chơi dở (không trống) -> Bật cái Pop-up lên
            if (backPopup != null)
            {
                backPopup.SetActive(true);
            }
        }
    }

    // ===============================================================
    // 3. GẮN VÀO NÚT "ĐỒNG Ý (YES)" Ở TRONG BẢNG POP-UP
    // ===============================================================
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hàm gắn vào nút Exit
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnClick_CancelExit()
    {
        // Bấm No hoặc X thì chỉ việc tắt cái bảng Pop-up đi để chơi tiếp
        if (backPopup != null)
        {
            backPopup.SetActive(false);
        }
    }
}