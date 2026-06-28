using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    [Header("Giao diện gốc")]
    public Image normalImage;
    public List<Sprite> normalImages;

    [Header("Trạng thái (Tập 8 & 9)")]
    public Image hoverImage;
    public Image activeImage;

    // Biến kiểm tra xem ô này đã có gạch nằm chưa
    public bool isOccupied { get; private set; } = false;

    void Start()
    {
        if (hoverImage != null) hoverImage.gameObject.SetActive(false);
        if (activeImage != null) activeImage.gameObject.SetActive(false);
    }

    // Hàm đổi màu cờ vua tập 1 của bạn (Giữ nguyên)
    public void SetImage(bool setFirstImage)
    {
        normalImage.sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    // Bật/tắt bóng mờ khi rê khối hình ngang qua (Tập 8)
    public void SetHover(bool state)
    {
        if (!isOccupied && hoverImage != null)
        {
            hoverImage.gameObject.SetActive(state);
        }
    }

    // Đặt gạch thật xuống ô này (Tập 9)
    public void ActivateSquare()
    {
        isOccupied = true;
        if (hoverImage != null) hoverImage.gameObject.SetActive(false);
        if (activeImage != null) activeImage.gameObject.SetActive(true);
    }
}