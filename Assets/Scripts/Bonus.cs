using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class ghép cặp Màu Gạch <-> GameObject Chữ Bonus tương ứng
[System.Serializable]
public class ColorBonusData
{
    public Sprite blockSprite;      // Kéo ảnh viên gạch màu (Xanh, Vàng, Cam...) vào đây
    public GameObject bonusObj;     // Kéo thẻ GameObject chữ tương ứng (Blue_Bonus...) vào đây
}

public class Bonus : MonoBehaviour
{
    public static Bonus Instance;

    [Header("Danh sách 8 cặp Màu - GameObject Bonus")]
    public List<ColorBonusData> colorBonusList;
    public int colorBonusScore = 50;

    private void Awake()
    {
        Instance = this;
    }

    // THÊM ĐOẠN NÀY VÀO TRONG CLASS BONUS:
    private void Update()
    {
        // Cứ đang chơi game mà gõ phím B trên bàn phím là tự động test!
        if (Input.GetKeyDown(KeyCode.B))
        {
            TestColorClearBonusCheat();
        }
    }

    // Hàm này sẽ được bàn cờ (Grid) gọi khi có 1 màu tuyệt chủng
    public void TriggerColorClearBonus(Sprite clearedColorSprite)
    {
        // 1. Thưởng 50 điểm
        if (Score.Instance != null)
        {
            Score.Instance.AddScore(colorBonusScore);
            Debug.Log("COLOR BONUS! Thưởng thêm " + colorBonusScore + " điểm!");
        }

        if (SoundEffect.Instance != null) SoundEffect.Instance.PlayBonus();

        // 2. Quét tìm GameObject chữ có màu tương ứng để bật lên
        foreach (var item in colorBonusList)
        {
            if (item.blockSprite == clearedColorSprite && item.bonusObj != null)
            {
                StartCoroutine(PlayBonusAnimation(item.bonusObj));
                break; // Tìm thấy và bật lên rồi thì dừng lại
            }
        }
    }

    // Coroutine bật chữ -> chờ 1.2 giây -> tắt đi
    private IEnumerator PlayBonusAnimation(GameObject bonusObj)
    {
        bonusObj.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        bonusObj.SetActive(false);
    }

    // ===================================================================
    // LỆNH GIAN LẬN: TẠO NÚT BẤM TEST TUYỆT CHỦNG MÀU NGOÀI INSPECTOR
    // ===================================================================
    [ContextMenu("Test Tuyet Chung Mau Ngay")]
    public void TestColorClearBonusCheat()
    {
        // 1. Kiểm tra xem game đã chạy và có màu hiện tại chưa
        if (Score.CurrentBlockColor != null)
        {
            Debug.Log("--- GIẢ LẬP DIỆT CHỦNG MÀU ĐANG CHƠI ---");

            // Truyền ĐÚNG màu đang chạy của hệ thống vào để kích hoạt!
            TriggerColorClearBonus(Score.CurrentBlockColor);
        }
        else
        {
            // 2. Phòng hờ nếu bạn bấm nút test lúc CHƯA PLAY GAME, 
            // nó sẽ lấy đại màu đầu tiên (số 0) để không bị lỗi crash game
            if (colorBonusList != null && colorBonusList.Count > 0)
            {
                Debug.Log("--- GAME CHƯA CHẠY: LẤY MẪU MÀU SỐ 0 ---");
                TriggerColorClearBonus(colorBonusList[0].blockSprite);
            }
        }
    }
}