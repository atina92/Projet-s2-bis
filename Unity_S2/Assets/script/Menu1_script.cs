using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Menu1_script : MonoBehaviour
{

    public void CloseApp()
    {
        Application.Quit();
    }

    public void ChangeMenu()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
