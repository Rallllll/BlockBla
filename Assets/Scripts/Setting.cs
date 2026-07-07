using UnityEngine;
using System.Collections;

public class Setting : MonoBehaviour
{
    [Header("Bảng Settings & Animator")]
    public GameObject settingsPanel;
    public Animator panelAnimator;

    [Header("Thời gian chạy Animation (Để khóa nút - giây)")]
    public float animDuration = 0.5f; // Nhìn trong Animator clip dài bao lâu thì gõ vào đây (VD: 0.5 hoặc 0.75)

    private bool isOpened = false;
    private bool isAnimating = false; // KHÓA: Đang chạy anim thì không cho bấm

    public void ToggleSettings()
    {
        if (SoundEffect.Instance != null)
        {
            SoundEffect.Instance.PlayClick();
        }
        // 1. NẾU ANIMATION ĐANG CHẠY -> TỪ CHỐI NHẬN LỆNH BẤM (Chống spam click)
        if (isAnimating || settingsPanel == null) return;

        // 2. Bắt đầu quy trình Mở hoặc Đóng (An toàn tuyệt đối)
        StartCoroutine(HandleToggleRoutine());
    }

    private IEnumerator HandleToggleRoutine()
    {
        isAnimating = true; // Khóa nút Bánh Răng lại
        isOpened = !isOpened; // Đảo trạng thái

        if (isOpened)
        {
            // --- CHIỀU MỞ RẠNG RỠ ---
            settingsPanel.SetActive(true);
            if (panelAnimator != null) panelAnimator.SetTrigger("Open");

            // Chờ animation xòe ra chạy xong
            yield return new WaitForSeconds(animDuration);
            // Xong xuôi: Bảng sẽ mở VĨNH VIỄN ở đây, không bao giờ tự tắt!
        }
        else
        {
            // --- CHIỀU ĐÓNG MƯỢT MÀ ---
            if (panelAnimator != null) panelAnimator.SetTrigger("Close");

            // Chờ animation thu nhỏ chạy xong
            yield return new WaitForSeconds(animDuration);

            // Chạy lùi xong hết rồi mới ẩn bảng đi cho sạch màn hình
            settingsPanel.SetActive(false);
        }

        isAnimating = false; // Mở khóa cho lần bấm tiếp theo!
    }
}