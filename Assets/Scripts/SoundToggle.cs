using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundToggle : MonoBehaviour
{
    public enum SoundType { Music, VFX }
    [Header("Loại âm thanh của nút này")]
    public SoundType soundType;

    [Header("Các thành phần UI")]
    public RectTransform handleIcon; // Kéo thẻ Handle_Icon con vào đây
    public Image backgroundImage;    // Kéo thẻ Image nền của chính Button này vào đây

    [Header("Hình ảnh nền (Đổi màu như Apple)")]
    public Sprite backgroundOn;      // Kéo ảnh nền Xanh lá (slice3)
    public Sprite backgroundOff;     // Kéo ảnh nền Xám trắng (slice1)

    [Header("Tọa độ cục tròn (Pos X)")]
    public float posX_On = 35f;      // Vị trí nằm bên phải
    public float posX_Off = -35f;    // Vị trí nằm bên trái
    public float moveDuration = 0.2f;// Thời gian lướt siêu nhanh (0.2 giây)

    private bool isOn = true;
    private Coroutine moveRoutine;

    private void Start()
    {
        // 1. Đọc trạng thái đã lưu (Mặc định lần đầu chơi là BẬT - true)
        string saveKey = (soundType == SoundType.Music) ? "SETTING_MUSIC" : "SETTING_VFX";
        isOn = PlayerPrefs.GetInt(saveKey, 1) == 1;

        // 2. Cập nhật giao diện ngay lập tức mà không cần chạy animation lướt
        UpdateUI(false);

        // 3. Cập nhật âm thanh hệ thống
        ApplySoundState();
    }

    // Hàm này sẽ được gắn vào sự kiện OnClick() của chính Button này
    public void OnToggleClicked()
    {
        isOn = !isOn; // Đảo trạng thái (Đang Bật -> Tắt, Đang Tắt -> Bật)

        // Lưu lại vào bộ nhớ máy
        string saveKey = (soundType == SoundType.Music) ? "SETTING_MUSIC" : "SETTING_VFX";
        PlayerPrefs.SetInt(saveKey, isOn ? 1 : 0);
        PlayerPrefs.Save();

        // Chạy hiệu ứng gạt lướt Apple
        UpdateUI(true);

        // Áp dụng tắt/bật loa
        ApplySoundState();
    }

    private void UpdateUI(bool animate)
    {
        // Đổi hình nền Xanh <-> Xám
        if (backgroundImage != null)
        {
            backgroundImage.sprite = isOn ? backgroundOn : backgroundOff;
        }

        // Tính tọa độ đích đến của cục tròn
        float targetX = isOn ? posX_On : posX_Off;

        if (animate && gameObject.activeInHierarchy)
        {
            // Nếu có animation -> Lướt êm ái
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveHandleRoutine(targetX));
        }
        else
        {
            // Nếu vừa mở game -> Dịch chuyển tức thời tới chỗ luôn
            if (handleIcon != null)
            {
                Vector2 pos = handleIcon.anchoredPosition;
                pos.x = targetX;
                handleIcon.anchoredPosition = pos;
            }
        }
    }

    // Coroutine tạo hiệu ứng lướt mượt mà như iOS
    private IEnumerator MoveHandleRoutine(float targetX)
    {
        float elapsed = 0f;
        Vector2 startPos = handleIcon.anchoredPosition;
        Vector2 endPos = new Vector2(targetX, startPos.y);

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            // Dùng SmoothStep để tốc độ lướt có trớn: nhanh ở giữa, chậm lúc bắt đầu và kết thúc
            float t = Mathf.SmoothStep(0f, 1f, elapsed / moveDuration);
            handleIcon.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        handleIcon.anchoredPosition = endPos;
    }

    // Liên kết với hệ thống âm thanh của game
    private void ApplySoundState()
    {
        if (soundType == SoundType.Music)
        {
            // NẾU LÀ NHẠC NỀN: Tìm nguồn phát nhạc và Tắt/Bật Mute
            // (Ví dụ bạn có thẻ AudioSource nhạc nền ngoài Scene thì quản lý ở đây)
            AudioListener.pause = !isOn; // Xài tạm lệnh này nếu bạn muốn tắt toàn bộ âm thanh

            // HOẶC chuẩn nhất: gọi sang SoundManager của bạn
            // if (SoundManager.Instance != null) SoundManager.Instance.ToggleMusic(isOn);
        }
        else
        {
            // NẾU LÀ VFX:
            // if (SoundManager.Instance != null) SoundManager.Instance.ToggleVFX(isOn);
        }
    }
}