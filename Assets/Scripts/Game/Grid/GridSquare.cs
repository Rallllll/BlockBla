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

    public Image redImage;

    public bool isOccupied { get; private set; } = false;

    void Start()
    {
        if (hoverImage != null) hoverImage.gameObject.SetActive(false);
        if (activeImage != null) activeImage.gameObject.SetActive(false);
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.sprite = setFirstImage ? normalImages[1] : normalImages[0];
    }

    public void SetHover(bool state)
    {
        if (!isOccupied && hoverImage != null)
        {
            hoverImage.gameObject.SetActive(state);
        }
    }

    public void ActivateSquare()
    {
        isOccupied = true;
        if (hoverImage != null) hoverImage.gameObject.SetActive(false);
        if (activeImage != null) activeImage.gameObject.SetActive(true);
    }

    public void SetErrorColor(bool isError)
    {
        if (redImage != null) redImage.gameObject.SetActive(isError);
    }

    // Tắt hình ảnh khối gạch đang nằm trên ô này
    public void Deactivate()
    {
        // Chú ý: Hãy sửa 'activeImage' thành tên biến hình ảnh gạch của bạn nếu bạn đặt tên khác
        activeImage.gameObject.SetActive(false);
    }

    // Xóa trạng thái chiếm chỗ
    public void ClearOccupied()
    {
        isOccupied = false;
        // Nếu bạn có biến nào khác lưu trạng thái đã đặt gạch thì reset luôn ở đây
    }
}