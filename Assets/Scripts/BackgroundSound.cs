using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public static BackgroundSound Instance;
    public AudioSource bgmSource;
    public AudioClip backgroundMusic;

    private void Awake()
    {
        // --- ĐÂY LÀ ĐOẠN GIÚP NHẠC BẤT TỬ ---
        if (Instance == null)
        {
            Instance = this;

            // LỆNH THẦN THÁNH: Nói với Unity là "Đừng phá hủy cục này khi load scene mới!"
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu sang scene khác mà lỡ có một cục SoundManager clone nào khác thì hủy thằng giả mạo đi
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Vừa vào game tự động bật nhạc nền
        if (bgmSource != null && backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
            bgmSource.loop = true; // Bắt buộc lặp lại
            bgmSource.Play();
        }
    }
}