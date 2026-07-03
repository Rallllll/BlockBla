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

    // Hàm này sẽ được bàn cờ (Grid) gọi khi có 1 màu tuyệt chủng
    public void TriggerColorClearBonus(Sprite clearedColorSprite)
    {
        // 1. Thưởng 50 điểm
        if (Score.Instance != null)
        {
            Score.Instance.AddScore(colorBonusScore);
            Debug.Log("COLOR BONUS! Thưởng thêm " + colorBonusScore + " điểm!");
        }

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
}