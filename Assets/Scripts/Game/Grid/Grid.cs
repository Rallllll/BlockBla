using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            float hitRadius = closestRect.rect.width * closestRect.lossyScale.x * 0.7f;

            if (minDistance <= hitRadius)
            {
                return closestSquare;
            }
        }
        return null;
    }

    public void CheckIfAnyLineIsCompleted()
    {
        List<GridSquare> squaresToClear = new List<GridSquare>();

        // 1. Quét HÀNG NGANG
        for (int row = 0; row < rows; row++)
        {
            bool isRowComplete = true;
            List<GridSquare> currentRowSquares = new List<GridSquare>();

            for (int col = 0; col < columns; col++)
            {
                GridSquare square = gridSquaresMatrix[col, row];
                currentRowSquares.Add(square);

                // Nếu có 1 ô trống -> Hàng này chưa đầy
                if (square == null || !square.isOccupied)
                {
                    isRowComplete = false;
                    break;
                }
            }

            // Nếu hàng đã đầy -> Đưa các ô này vào danh sách "Chờ tử hình"
            if (isRowComplete)
            {
                squaresToClear.AddRange(currentRowSquares);
            }
        }

        // 2. Quét CỘT DỌC
        for (int col = 0; col < columns; col++)
        {
            bool isColComplete = true;
            List<GridSquare> currentColSquares = new List<GridSquare>();

            for (int row = 0; row < rows; row++)
            {
                GridSquare square = gridSquaresMatrix[col, row];
                currentColSquares.Add(square);

                // Nếu có 1 ô trống -> Cột này chưa đầy
                if (square == null || !square.isOccupied)
                {
                    isColComplete = false;
                    break;
                }
            }

            // Nếu cột đã đầy -> Đưa các ô này vào danh sách "Chờ tử hình"
            if (isColComplete)
            {
                squaresToClear.AddRange(currentColSquares);
            }
        }

        // 3. XỬ TRẢM (Xóa sạch các ô đã lọt vào danh sách)
        if (squaresToClear.Count > 0)
        {
            foreach (var square in squaresToClear)
            {
                square.Deactivate();    // Tắt hình
                square.ClearOccupied(); // Trả lại chỗ trống
            }

            // --- THÊM LOGIC TÍNH ĐIỂM Ở ĐÂY ---
            // Cứ 1 hàng (hoặc 1 cột) bị xóa là có 9 ô. Tính số hàng bị xóa:
            int linesCleared = squaresToClear.Count / columns;

            // Điểm = 10 điểm cho 1 hàng. Xóa 2 hàng 1 lúc (Combo) thì được x2 (10 * 2 = 20)
            int scoreToReward = linesCleared * 10;

            // Bắn tín hiệu sang ScoreManager để cộng điểm
            if (Score.Instance != null)
            {
                Score.Instance.AddScore(scoreToReward);
            }
        }
        CheckGameOver();
    }

    // --- THUẬT TOÁN KIỂM TRA GAME OVER (TỐI ƯU HÓA) ---
    public void CheckGameOver()
    {
        // 1. Lấy danh sách các khối gạch đang có trên khay
        var shapeStorage = Object.FindFirstObjectByType<ShapeStorage>();
        if (shapeStorage == null) return;

        bool canPlaceAnyShape = false;

        // 2. Thử từng khối gạch trên khay
        foreach (var shape in shapeStorage.shapeList)
        {
            if (!shape.gameObject.activeSelf) continue; // Bỏ qua khối đã đặt rồi

            List<Vector2Int> shapeMap = shape.GetShapeMap();

            // 3. Quét toàn bộ bàn cờ xem có chỗ nào nhét vừa không
            if (CanShapeFitOnGrid(shapeMap))
            {
                canPlaceAnyShape = true;
                break; // Chỉ cần 1 khối đặt được là CHƯA THUA, thoát vòng lặp luôn
            }
        }

        // 4. Nếu không thể đặt bất kỳ khối nào -> GAME OVER
        if (!canPlaceAnyShape)
        {
            Debug.Log("GAME OVER! KHÔNG THỂ ĐẶT THÊM GẠCH!");
            // (Tập sau sẽ gọi màn hình thua ở đây)
        }
    }

    // Hàm đi ướm thử 1 khối gạch vào toàn bộ bàn cờ
    private bool CanShapeFitOnGrid(List<Vector2Int> shapeMap)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Thử đặt khối gạch vào gốc tọa độ (col, row)
                bool canFitHere = true;

                foreach (var blockOffset in shapeMap)
                {
                    int checkCol = col + blockOffset.x;
                    int checkRow = row + blockOffset.y;

                    // Nếu gạch thò ra ngoài bàn cờ -> Không vừa
                    if (checkCol >= columns || checkRow >= rows)
                    {
                        canFitHere = false;
                        break;
                    }

                    // Nếu đụng phải ô đã có gạch -> Không vừa
                    GridSquare targetSquare = gridSquaresMatrix[checkCol, checkRow];
                    if (targetSquare == null || targetSquare.isOccupied)
                    {
                        canFitHere = false;
                        break;
                    }
                }

                // Nếu quét xong mà vẫn canFitHere == true, tức là NHÉT VỪA MỘT CHỖ!
                if (canFitHere)
                {
                    return true;
                }
            }
        }
        return false;
    }
}