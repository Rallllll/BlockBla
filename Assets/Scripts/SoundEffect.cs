using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    // Singleton riêng biệt cho SFX
    public static SoundEffect Instance;

    [Header("Loa phát hiệu ứng (Audio Source)")]
    public AudioSource sfxSource;

    [Header("--- KHO ÂM THANH HIỆU ỨNG ---")]
    public AudioClip clickClip;       // Tiếng bấm nút (Play, Setting, Out...)
    public AudioClip toggleClip;
    public AudioClip endGameClip;     // Tiếng thua game (Game Over)
    public AudioClip newBestClip;     // Tiếng phá kỷ lục (New Best)
    public AudioClip[] praiseClips;   // Danh sách từ khen (Good, Excellent, Amazing...)

    private bool isVFXOn = true;      // Biến ghi nhớ trạng thái Bật/Tắt

    private void Awake()
    {
        // 1. Setup Singleton & Bất tử qua các Scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 2. Đọc bộ nhớ xem người chơi đang Bật hay Tắt VFX (Mặc định 1 là Bật)
        isVFXOn = PlayerPrefs.GetInt("SETTING_VFX", 1) == 1;
    }

    // --- HÀM GỐC: PHÁT ÂM THANH CHỒNG NHAU (PLAY ONE SHOT) ---
    public void PlayClip(AudioClip clip)
    {
        // Nếu đang tắt VFX, hoặc không có loa, hoặc không có clip -> Bỏ qua ngay
        if (!isVFXOn || sfxSource == null || clip == null) return;

        // PlayOneShot giúp nghe được nhiều tiếng cùng lúc (VD: Vừa ăn gạch vừa nổ combo)
        sfxSource.PlayOneShot(clip);
    }

    // --- CÁC HÀM GỌI NHANH CHO TỪNG SỰ KIỆN ---

    // 1. Gọi khi bấm nút UI
    public void PlayClick() => PlayClip(clickClip);

    // 2. Gọi khi Game Over
    public void PlayEndGame() => PlayClip(endGameClip);

    // 3. Gọi khi đạt điểm Kỷ lục mới
    public void PlayNewBest() => PlayClip(newBestClip);

    public void PlayClip() => PlayClip(toggleClip);

    // 4. Gọi khi ăn điểm/combo (Đọc ngẫu nhiên 1 từ khen ngợi)
    public void PlayRandomPraise()
    {
        if (praiseClips == null || praiseClips.Length == 0) return;

        int randomIndex = Random.Range(0, praiseClips.Length);
        PlayClip(praiseClips[randomIndex]);
    }

    // --- HÀM CÔNG TẮC (Nối với nút gạt VFX ngoài Settings) ---
    public void ToggleVFX(bool isOn)
    {
        isVFXOn = isOn;
        // Lưu trạng thái mới vào bộ nhớ
        PlayerPrefs.SetInt("SETTING_VFX", isVFXOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}