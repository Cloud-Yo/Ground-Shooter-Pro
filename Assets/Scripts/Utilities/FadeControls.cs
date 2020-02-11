using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeControls : MonoBehaviour
{
    [SerializeField] private Animator _myAnimator;
     void Start()
    {
      
        _myAnimator = GetComponent<Animator>();
        FadeIn();
    }



    public void FadeOut()
    {
        _myAnimator.SetTrigger("fadeOut");
        
    }

    public void FadeIn()
    {
        _myAnimator.SetTrigger("fadeIn");
    }
}
