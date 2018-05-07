using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControll : MonoBehaviour {

    public GameObject MainPanel, JointPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && JointPanel.activeSelf  && !MainPanel.activeSelf)
        {
            JointPanel.SetActive(false);
            MainPanel.SetActive(true);
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
