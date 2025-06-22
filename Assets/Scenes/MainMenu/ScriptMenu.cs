using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditorInternal;


public class ScriptMenu : MonoBehaviour
{

    [Header("Main Menu Scene")]
    [SerializeField] GameObject pengaturanPanel;
    [SerializeField] GameObject menuPanel;


    [Header("Transisi")]
    public Animator transisi;
    [SerializeField] float waktuTransisi = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        pengaturanPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void PengaturanButton()
    {
        pengaturanPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void KeluarPengaturan()
    {
        pengaturanPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void Keluar()
    {
        Application.Quit();
        Debug.Log("Sudah Keluar Aplikasi!");
    }



    //TRANSISI
    
}
