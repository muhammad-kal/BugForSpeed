using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKeluar : MonoBehaviour
{
    public void KeluarGame()
    {
        Debug.Log("Game Keluar");
        Application.Quit();
    }
}
