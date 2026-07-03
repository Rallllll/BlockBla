using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combo : MonoBehaviour
{
    public static Combo Instance;

    [Header("Kéo tất cả các thẻ Combo_Awesome, Cool... vào đây")]
    public List<GameObject> comboWritings;

    private void Awake()
    {
        Instance = this;
    }

    // Hàm gọi random 1 chữ hiện lên
    public void ShowRandomComboText()
    {
        if (comboWritings == null || comboWritings.Count == 0) return;

        // Bốc ngẫu nhiên 1 chữ trong danh sách
        int randomIndex = Random.Range(0, comboWritings.Count);
        StartCoroutine(PlayAnimation(comboWritings[randomIndex]));
    }

    // Coroutine tự động bật chữ, chờ chạy xong Anim rồi tự giấu đi
    private IEnumerator PlayAnimation(GameObject textObj)
    {
        textObj.SetActive(true); // Bật lên là Anim tự chạy
        yield return new WaitForSeconds(1.2f); // Chờ Anim chạy xong
        textObj.SetActive(false); // Tự tắt đi chờ lần combo sau
    }
}