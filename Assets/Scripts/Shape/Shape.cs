using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public GameObject squareShapeImage;

    [HideInInspector]
    public ShapeData CurrentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();

    [Header("Drag Settings")]
    public Vector3 shapeSelectedScale;
    public Vector2 offset;             
    private Vector3 shapeStartScale;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        shapeStartScale = this.transform.localScale;
        startLocalPosition = rectTransform.localPosition;
    }

    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.transform.localScale = shapeSelectedScale;
        this.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        rectTransform.localPosition = localPoint + offset;

        if (Grid.Instance == null) return;

        // Xóa hết toàn bộ bóng mờ cũ trên Grid trước
        foreach (var square in Grid.Instance.gridSquaresMatrix)
        {
            if (square != null) square.SetHover(false);
        }

        // Quét từng viên gạch con để bật bóng mờ tương ứng
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            // Truyền thẳng tọa độ thực tế (child.position) vào, KHÔNG dùng ScreenPoint nữa
            GridSquare targetSquare = Grid.Instance.GetGridSquareAtPosition(child.position);

            if (targetSquare != null)
            {
                targetSquare.SetHover(true);
            }
        }
    }

    // Xử lý thả tay ra và hít gạch vào lưới (Tập 9)
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = shapeStartScale;
        List<GridSquare> targetSquares = new List<GridSquare>();
        bool canPlace = true;

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            // Dùng lưới nam châm để tìm ô
            GridSquare target = Grid.Instance.GetGridSquareAtPosition(child.position);

            // Kiểm tra logic: Phải có ô, ô phải trống, và ô đó không được trùng với ô đã chọn
            if (target == null || target.isOccupied || targetSquares.Contains(target))
            {
                canPlace = false;
                break;
            }
            targetSquares.Add(target);
        }

        if (canPlace)
        {
            foreach (var square in targetSquares) square.ActivateSquare();
            gameObject.SetActive(false); // Đặt xong thì ẩn khối đi
        }
        else
        {
            // Thất bại: Dọn sạch bóng mờ và bay về ĐÚNG VỊ TRÍ GỐC
            foreach (var square in Grid.Instance.gridSquaresMatrix)
            {
                if (square != null) square.SetHover(false);
            }

            // SỬA Ở ĐÂY: Bay về vị trí "nhà" đã lưu từ đầu
            rectTransform.localPosition = startLocalPosition;
        }
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        CreateShape(shapeData);
    }    

    public void CreateShape(ShapeData shapeData)
    {
        CurrentShapeData = shapeData;
        var totalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count <= totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect =  squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2 (squareRect.rect.width * squareRect.localScale.x, 
            squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        // set position to form final shape
        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column]) 
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition 
                        = new Vector2(GetXPositionForShapeSquare(shapeData, column, moveDistance), 
                                    GetYPositionForShapeSquare(shapeData,row, moveDistance));

                    currentIndexInList++;      
                }
            }
        }
    }    

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            float startXPos;
            if (shapeData.columns % 2 != 0)
                startXPos = (shapeData.columns / 2) * moveDistance.x * -1;
            else
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
            shiftOnX = startXPos + column * moveDistance.x;

        }
        return shiftOnX;
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            float startYPos;
            if (shapeData.rows % 2 != 0)
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            else
                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int numbers = 0;

        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                {
                    numbers++;
                }
            }
        }

        return numbers;
    }
}
