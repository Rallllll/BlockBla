using UnityEngine;
using UnityEngine.SceneManagement;

public class Infor : MonoBehaviour
{
    [Header("--- CÀI ĐẶT UI ---")]
    public GameObject inFor; // Kéo cái bảng Pop-up bồ đã làm sẵn vào đây

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // 1. Vừa vào game là tự động ẩn cái Pop-up đi
        if (inFor != null)
        {
            inFor.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_InforButton()
    {
        Debug.Log("ĐÃ BẤM NÚT MỞ! Đang kiểm tra biến inFor..."); // Thêm dòng này

        if (inFor != null)
        {
            inFor.SetActive(true);
            Debug.Log("Mở Pop-up thành công!"); // Thêm dòng này
        }
    }


    public void OnClick_CancelExit()
    {
        // Bấm No hoặc X thì chỉ việc tắt cái bảng Pop-up đi để chơi tiếp
        if (inFor != null)
        {
            inFor.SetActive(false);
        }
    }
}
