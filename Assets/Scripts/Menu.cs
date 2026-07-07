using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void LoadScene(string name)
    {
        if (SoundEffect.Instance != null)
        {
            SoundEffect.Instance.PlayClick();
        }
        SceneManager.LoadScene(name);
    }
}
