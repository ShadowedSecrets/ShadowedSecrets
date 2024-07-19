using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleManager : MonoBehaviour
{
    public GameObject riddlePanel;

    void Update()
    {
        if (riddlePanel.activeInHierarchy && Input.GetKeyDown(KeyCode.C))
        {
            riddlePanel.SetActive(!riddlePanel.activeSelf);
        }
    }

    public void ShowText(string text)
    {
        ShowRiddlePanel();
        riddlePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }

    public void ShowRiddlePanel()
    {
        riddlePanel.SetActive(true);
    }
}


