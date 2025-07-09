using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene_Manager : MonoBehaviour
{
    public GameObject Billboard_Panel;
    public GameObject Koleksi_Panel;
    public GameObject Toko_Panel;
    public GameObject Balap_Panel;

    public void Billboard_Select()
    {
        Billboard_Panel.SetActive(true);
        Koleksi_Panel.SetActive(false);
        Toko_Panel.SetActive(false);
        Balap_Panel.SetActive(false);
    }

    public void Koleksi_Select()
    {
        Billboard_Panel.SetActive(false);
        Koleksi_Panel.SetActive(true);
        Toko_Panel.SetActive(false);
        Balap_Panel.SetActive(false);
    }

    public void Balap_Select()
    {
        Billboard_Panel.SetActive(false);
        Koleksi_Panel.SetActive(false);
        Toko_Panel.SetActive(false);
        Balap_Panel.SetActive(true);
    }

    public void Toko_Select()
    {
        Billboard_Panel.SetActive(false);
        Koleksi_Panel.SetActive(false);
        Toko_Panel.SetActive(true);
        Balap_Panel.SetActive(false);
    }



}
