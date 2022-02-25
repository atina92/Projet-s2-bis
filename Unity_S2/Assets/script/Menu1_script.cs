using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1_script : MonoBehaviour
{

    public void CloseApp()
    {
        Application.Quit();
    }

    public void ChangeMenu()
    {
        SceneManager.LoadScene(1);
    }
}
