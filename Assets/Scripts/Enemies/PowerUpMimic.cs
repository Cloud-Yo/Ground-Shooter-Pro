using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMimic : MonoBehaviour
{
    [SerializeField] private Sprite[] _mySprite = new Sprite[6];
    [SerializeField] private SpriteRenderer _mySR;
    [SerializeField] private Animator _myAnim;
    [SerializeField] private int _randSprite;
    [SerializeField] private GameObject _myPlayer;
    [SerializeField] private float _playerDist;
    [SerializeField] private float _revealDist = 4f;
    [SerializeField] private AudioClip _mimicClip;
    [SerializeField] private bool _revealed = false;

    private void Awake()
    {

        _myPlayer = GameObject.Find("Player");
        
        _mySR = GetComponent<SpriteRenderer>();
        _randSprite = Random.Range(0, 6);
        _mySR.sprite = _mySprite[_randSprite];
        _myAnim = GetComponent<Animator>();
        _myAnim.SetInteger("Type", _randSprite);


    }

    private void Update()
    {
        _playerDist = (this.transform.position - _myPlayer.transform.position).magnitude;

        if (_playerDist < _revealDist)
        {
            _myAnim.SetBool("Reveal", true);
            if(!_revealed)
            {
                AudioSource.PlayClipAtPoint(_mimicClip, Camera.main.transform.position, 0.35f);
                _revealed = true;
                GetComponentInParent<PowerUp>().BecomeEnemy();
            }
            
        }
    }

}
