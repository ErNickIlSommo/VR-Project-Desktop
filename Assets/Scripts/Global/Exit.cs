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

    private bool _canExit = false;
    
    public bool CanExit { get { return _canExit; }
        set
        {
            _canExit = value;
            var collider = GetComponent<BoxCollider>();
            collider.isTrigger = value;
        } 
    }

    /*private void Start()
    {
        fader.FadeIn();
    }*/

    private void Awake()
    {
        var col = GetComponent<BoxCollider>();
        col.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; 
        
        //transition.SetTrigger("Guard");
        if (_canExit)
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
