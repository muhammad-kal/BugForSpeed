using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [Header("Animasi")]
    public Animator transisi;
    public float waktuTransisi = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartButton(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadLevel(sceneName));
    }


    IEnumerator LoadLevel(string sceneName)
    {
        transisi.SetTrigger("Start");

        yield return new WaitForSeconds(waktuTransisi);

        SceneManager.LoadScene(sceneName);
    }

}
