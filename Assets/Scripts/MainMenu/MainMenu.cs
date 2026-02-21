using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MasterInitializer _masterInitializer;
    private void Awake()
    {
        _masterInitializer = GetComponent<MasterInitializer>();
    }

    public void PlayGame()
    {
        _masterInitializer.Initialize();
        SceneManager.LoadScene("Alveare");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
