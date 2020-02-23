using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameObject _hpNeedle;
    [SerializeField] private float _rotation;
    [SerializeField] private Color _underCol, _defaultCol, _newCol;
    [SerializeField] private float _Health = 100f;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _speed, _lerpSpeed;
    [SerializeField] private int _lifeCount = 3;
    [SerializeField] private Animator _hpAnimator;
    [SerializeField] private GameObject[] _lifeLights;
    [SerializeField] private Player _Player;
    [SerializeField] private SpriteRenderer _myRender;
    [SerializeField] private SpawnManager _mySpawnManager;
    [SerializeField] private GameObject _leftSparks, _rightSparks;


    
    void Start()
    {
        _Player = GameObject.Find("Player").GetComponent<Player>();
        _lifeCount = 3;
        _currentHealth = 100;
        _rotation = 0f;
        _hpNeedle = GameObject.Find("tankUI_Needle");
        _myRender = GetComponent<SpriteRenderer>();
        _newCol = _myRender.color;
        _mySpawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        

    }

  
    void Update()
    {
        Vector3 _currentAngle = _hpNeedle.transform.eulerAngles;
        
        _myRender.color = Color.Lerp(_myRender.color, _newCol, _lerpSpeed * Time.deltaTime);
        
        
        if (_hpNeedle.transform.eulerAngles.z != _rotation)
        {
            _hpNeedle.transform.eulerAngles = Vector3.RotateTowards(_currentAngle, new Vector3(0, 0, _rotation), _speed * Time.deltaTime, _speed) ;
        }

        if (_currentHealth <= 0)
        {
            _rotation = 180;

            if (_lifeCount > 1 && _hpNeedle.transform.eulerAngles.z == 180)
            {
                _currentHealth = 100;
                _lifeCount--;
                _lifeLights[_lifeCount].SetActive(false);
                _rotation = 0;
            }
            else if (_lifeCount == 1)
            {
                _rotation = 180;
                _lifeCount--;
                _lifeLights[_lifeCount].SetActive(false);
                _Player.OnPlayerDeath();

                _mySpawnManager._playerAlive = false;
            }


        }

        if(_currentHealth <= 30)
        {
            _hpAnimator.SetBool("warning", true);
        }
        else if(_currentHealth > 30)
        {
            _hpAnimator.SetBool("warning", false);
        }

        if(_lifeCount == 2)
        {
            if(_Player != null)
            {
                _leftSparks.SetActive(true);
            }
            
        }
        else if(_lifeCount == 1)
        {
            if(_Player != null)
            {
                _rightSparks.SetActive(true);
            }
            
        }
        
    }

    public void UpdateHealth(int dam)
    {
        _currentHealth -= dam;
        float _newRot = (_currentHealth * 180) * .01f;
        if (_rotation < 179f)
        {
            _rotation = 180 - _newRot;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _newCol = _underCol;
            Debug.Log("Player is under UI");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _newCol = _defaultCol;
        }
    }

    public void AddLives()
    {

        int hpDiff = (100 - _currentHealth) * -1;
        UpdateHealth(hpDiff);
        if (_lifeCount < 3)
        {
            _lifeCount++;
            _lifeLights[_lifeCount -1].SetActive(true);

        }
 
    }
}
