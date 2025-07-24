using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupHelp : MonoBehaviour
{
    public GameObject helpPanel;
    public float showTime = 10f;

    void Start()
    {
        helpPanel.SetActive(true);
        StartCoroutine(HidePanelAfterSeconds());
    }

    IEnumerator HidePanelAfterSeconds()
    {
        yield return new WaitForSeconds(showTime);
        helpPanel.SetActive(false);
    }
}
