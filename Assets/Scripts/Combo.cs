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

    //test tiếng
    private void Update()
    {
        // Cứ gõ phím SPACE trên bàn phím là nổ Combo!
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowRandomComboText();
        }
    }

    // Hàm gọi random 1 chữ hiện lên
    public void ShowRandomComboText()
    {
        if (comboWritings == null || comboWritings.Count == 0) return;

        // 1. Bốc 1 con số ngẫu nhiên duy nhất (Ví dụ ra số 1)
        int randomIndex = Random.Range(0, comboWritings.Count);

        // 2. Nói ông SoundEffect phát file âm thanh ở vị trí số 1 (Tiếng Cool)
        if (SoundEffect.Instance != null)
        {
            SoundEffect.Instance.PlayPraiseByIndex(randomIndex);
        }

        // 3. Bật cái chữ ở vị trí số 1 lên màn hình (Chữ Cool)
        StartCoroutine(PlayAnimation(comboWritings[randomIndex]));
    }

    // Coroutine tự động bật chữ, chờ chạy xong Anim rồi tự giấu đi
    private IEnumerator PlayAnimation(GameObject textObj)
    {
        textObj.SetActive(true); // Bật lên là Anim tự chạy
        yield return new WaitForSeconds(1.0f); // Chờ Anim chạy xong
        textObj.SetActive(false); // Tự tắt đi chờ lần combo sau
    }
}