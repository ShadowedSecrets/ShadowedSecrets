using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleManager : MonoBehaviour
{
    public GameObject riddlePanel;
    private bool canTogglePanel = false;

    void Update()
    {
        if (canTogglePanel && Input.GetKeyDown(KeyCode.C))
        {
            riddlePanel.SetActive(!riddlePanel.activeSelf);
        }
    }

    public void ShowRiddlePanel()
    {
        riddlePanel.SetActive(true);
        canTogglePanel = true;
    }
}


