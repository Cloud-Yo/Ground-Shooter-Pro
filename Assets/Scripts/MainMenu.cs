using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    [SerializeField] private Button _StartBTN;
    [SerializeField] private Button _QuitBTN;
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _SelectClip;


    void Start()
    {
        _myAS = GetComponent<AudioSource>();
    }

    
    public void StartMyGame()
    {
        _myAS.PlayOneShot(_SelectClip);
        SceneManager.LoadScene(1);

        
    }

    public void QuitMyGame()
    {
        Application.Quit();
    }

}
