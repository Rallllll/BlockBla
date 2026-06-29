using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData; 
    public List<Shape> shapeList;

    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            // Lưu ý: Mình đổi CreateShape thành RequestNewShape để nó đồng bộ với lỗi tàng hình bạn vừa sửa
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }

    // VIẾT THÊM HÀM NÀY: Để nạp gạch mới khi đang chơi
    public void CheckIfNeedNewShapes()
    {
        // 1. Quét xem khay đã trống hẳn chưa
        bool isTrayEmpty = true;
        foreach (var shape in shapeList)
        {
            if (shape.gameObject.activeSelf)
            {
                isTrayEmpty = false;
                break;
            }
        }

        // 2. Nếu khay trống trơn -> Đẻ 3 khối mới
        if (isTrayEmpty)
        {
            foreach (var shape in shapeList)
            {
                ShapeData randomData = shapeData[Random.Range(0, shapeData.Count)];
                shape.RequestNewShape(randomData);
            }
        }
    }
}