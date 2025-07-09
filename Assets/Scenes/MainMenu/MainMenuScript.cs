using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField] public Scene ContohScene;
    public SceneAsset ContohSceneAsset;
    public GameObject transisi;
    public GameObject penghalang;

    public void MULAI()
    {
        transisi.SetActive(true);
    }

    public void GantiScene()
    {
        penghalang.SetActive(true);
        SceneManager.LoadScene(ContohSceneAsset.name);
        
    }
}
