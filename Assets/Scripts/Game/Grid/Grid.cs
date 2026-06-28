using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    // Mẫu Singleton để file Shape gọi qua lại dễ dàng
    public static Grid Instance;

    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();

    // Ma trận 2D quản lý logic toán học của 64 ô
    public GridSquare[,] gridSquaresMatrix;
    private Canvas canvas;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        CreateGrid();
    }

    private void CreateGrid()
    {
        gridSquaresMatrix = new GridSquare[columns, rows];
        SpawnGridSquares();
        SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; ++column)
            {
                GameObject newSquare = Instantiate(gridSquare) as GameObject;
                _gridSquares.Add(newSquare);

                newSquare.transform.SetParent(this.transform);
                newSquare.transform.localScale = new Vector3(squareScale, squareScale, squareScale);

                GridSquare gridSquareScript = newSquare.GetComponent<GridSquare>();
                gridSquareScript.SetImage(square_index % 2 == 0);

                // Lưu ô lưới vào ma trận tọa độ [X, Y]
                gridSquaresMatrix[column, row] = gridSquareScript;

                square_index++;
            }
        }
    }

    // Hàm tính tọa độ chia khoảng cách của bạn (Giữ nguyên 100%)
    private void SetGridSquaresPositions()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (column_number + 1 > columns)
            {
                square_gap_number.x = 0.0f;
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.x * row_number + (square_gap_number.y * squaresGap);

            if (column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }

            if (row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset,
                startPosition.y - pos_y_offset);

            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset,
                startPosition.y - pos_y_offset, 0.0f);

            column_number++;
        }
    }

    // Thuật toán quét xem tọa độ màn hình có nằm đè lên ô lưới nào không
    public GridSquare GetGridSquareAtPosition(Vector3 brickWorldPosition)
    {
        GridSquare closestSquare = null;
        float minDistance = float.MaxValue;

        // Quét toàn bộ bảng lưới tìm ô gần viên gạch nhất
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (gridSquaresMatrix[col, row] != null)
                {
                    Transform squareTransform = gridSquaresMatrix[col, row].transform;

                    // Tính khoảng cách thực tế giữa tâm viên gạch và tâm ô lưới
                    float dist = Vector3.Distance(squareTransform.position, brickWorldPosition);

                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestSquare = gridSquaresMatrix[col, row];
                    }
                }
            }
        }

        // Kiểm tra xem viên gạch có nằm trong vùng nhận diện không
        if (closestSquare != null)
        {
            RectTransform closestRect = closestSquare.GetComponent<RectTransform>();

            // Bán kính hít: Bằng 70% bề ngang thực tế của ô lưới (Đủ rộng để bù đắp các khe hở Gap)
            float hitRadius = closestRect.rect.width * closestRect.lossyScale.x * 3f;

            if (minDistance <= hitRadius)
            {
                return closestSquare;
            }
        }
        return null;
    }
}