using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void LoadScene(string name)
    {
        if (SoundEffect.Instance != null)
        {
            SoundEffect.Instance.PlayClick();
        }
        SceneManager.LoadScene(name);
    }
    // ========================================================
    // HÀM GẮN VÀO NÚT EXIT (THOÁT GAME)
    // ========================================================
    public void ExitGame()
    {
        // 1. Phát tiếng bấm nút cho chuyên nghiệp
        if (SoundEffect.Instance != null) SoundEffect.Instance.PlayClick();

        Debug.Log("Đang thoát game...");

        // 2. Lệnh thoát game thông minh 2 trong 1:
#if UNITY_EDITOR
        // Nếu đang ngồi test trong phần mềm Unity -> Tắt nút Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Nếu đã build ra game thật (Android/PC/iOS) -> Tắt hẳn ứng dụng!
            Application.Quit();
#endif
    }
}
