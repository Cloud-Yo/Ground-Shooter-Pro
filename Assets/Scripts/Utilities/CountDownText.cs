using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownText : MonoBehaviour
{
    [SerializeField] private Text _myTxt;
    [SerializeField] private int _myNum = 4;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private GameManager _myGM;

    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _CountDownClip, _StartClip;

    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAS = GetComponent<AudioSource>();
        _myTxt = GetComponent<Text>();
        _myGM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void CountDownMethod()
    {
        if(_myNum == 0)
        {
            _myAnimator.StopPlayback();
            _myGM.SetGlobalSpeed(0.5f);
            Destroy(this.gameObject);
        }
        else if(_myNum == 1)
        {
            _myNum--;
            _myTxt.text = _myNum.ToString();
            _myAS.PlayOneShot(_StartClip);
        }
        else if(_myNum > 1)
        {
            _myNum--;
            _myTxt.text = _myNum.ToString();
            _myAS.PlayOneShot(_CountDownClip);
        }
    }
}
