using UnityEngine;
using UnityEngine.UI; // dieu khien ui button
using TMPro;          // text tmp

public class HelpBoosterManager : MonoBehaviour
{
    [Header("--- KẾT NỐI HỆ THỐNG ---")]
    public ShapeStorage shapeStorage;

    [Header("--- SỐ LƯỢNG LƯỢT DÙNG ---")]
    public int refreshCount = 3;
    public int undoCount = 3;
    public int bombCount = 2;

    [Header("--- KÉO TEXT (TMP) HIỂN THỊ SỐ LƯỢNG VÀO ĐÂY ---")]
    public TextMeshProUGUI refreshText;
    public TextMeshProUGUI undoText;
    public TextMeshProUGUI bombText;

    [Header("--- KÉO 3 NÚT BUTTON VÀO ĐÂY (Để khóa khi hết lượt) ---")]
    public Button refreshButton;
    public Button undoButton;
    public Button bombButton;

    private void Start()
    {
        // bat dau game la co nut
        UpdateAllBoosterUI();
    }

    // ===============================================================
    // HÀM CẬP NHẬT GIAO DIỆN (GỌI MỖI KHI DÙNG BOOSTER)
    // ===============================================================
    private void UpdateAllBoosterUI()
    {
        // 1. Cập nhật con số lên Text TMP
        if (refreshText != null) refreshText.text = refreshCount.ToString();
        if (undoText != null) undoText.text = undoCount.ToString();
        if (bombText != null) bombText.text = bombCount.ToString();

        // 2. Nếu hết lượt (Count <= 0) -> Khóa nút lại, làm tối màu nút đi!
        if (refreshButton != null) refreshButton.interactable = (refreshCount > 0);
        if (undoButton != null) undoButton.interactable = (undoCount > 0);
        if (bombButton != null) bombButton.interactable = (bombCount > 0);
    }

    // Các hàm để gắn vào On click của UI button
    public void OnClick_RefreshBooster()
    {
        if (SoundEffect.Instance != null) SoundEffect.Instance.PlayClick();

        if (refreshCount <= 0)
        {
            if (SoundEffect.Instance != null) SoundEffect.Instance.PlayError();
            return;
        }
        if (shapeStorage != null) 
            shapeStorage.Logic_RefreshShapes();

        refreshCount--;
        UpdateAllBoosterUI(); // dùng xong là cập nhật số lượng
    }

    public void OnClick_UndoBooster()
    {
        if (undoCount <= 0)
        {
            if (SoundEffect.Instance != null) SoundEffect.Instance.PlayError();
            return;
        }// Hết sạch lượt thì không chạy tiếp

        if (Grid.Instance != null)
        {
            // 💡 HỎI GRID: "Lượt này có Undo được thật không?"
            bool undoSuccess = Grid.Instance.Logic_UndoLastMove();

            // NẾU GRID TRẢ VỀ TRUE -> MỚI PHÁT CLICK VÀ TRỪ LƯỢT!
            if (undoSuccess)
            {
                if (SoundEffect.Instance != null) SoundEffect.Instance.PlayClip();

                undoCount--; //  Trừ lượt ở đây cực kỳ an toàn
                UpdateAllBoosterUI(); // Cập nhật số hiển thị
            }
            else
            {
                // (Tùy chọn) Phát tiếng âm thanh báo lỗi nhẹ nếu người chơi bấm cố lúc không được phép
                if (SoundEffect.Instance != null) SoundEffect.Instance.PlayError();
            }
        }
    }

    public void OnClick_BombBooster()
    {
        if (bombCount <= 0)
        {
            if (SoundEffect.Instance != null) SoundEffect.Instance.PlayError();
            return;
        }

        if (Grid.Instance != null)
        {
            if (Grid.Instance.IsGridEmpty())
            {
                Debug.Log("Bàn cờ đang trống trơn, bấm Bom làm gì cho phí bồ ơi!");

                // (Tùy chọn) Có thể phát tiếng tít tít báo lỗi ở đây nếu muốn
                if (SoundEffect.Instance != null) SoundEffect.Instance.PlayError();

                return; // ❌ Chặn đứng luồng code tại đây, không cho chạy xuống đoạn trừ lượt ở dưới!
            }
        } 

        if (SoundEffect.Instance != null) SoundEffect.Instance.PlayClick();

        Grid.Instance.Logic_ExplodeClearAll_Immediate();

        bombCount--;
        UpdateAllBoosterUI();
    }
}