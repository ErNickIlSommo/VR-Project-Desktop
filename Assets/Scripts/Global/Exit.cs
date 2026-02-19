using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Exit : MonoBehaviour
{

    [SerializeField] private string sceneName;
    // [SerializeField] private Fader fader;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 2f;

    /*private void Start()
    {
        fader.FadeIn();
    }*/

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        
        //transition.SetTrigger("Guard");
        StartCoroutine(LoadLevel());
    }
    

    private IEnumerator LoadLevel()
    {
        // yield return fader.FadeOut();
        transition.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime);
        
        // yield return fader.FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}
