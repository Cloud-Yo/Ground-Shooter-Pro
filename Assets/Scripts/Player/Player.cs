using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMult = 2f;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _speedTimer = 3.0f;
    [SerializeField] private float _speedBoostTime = 3.0f;
    [SerializeField] private int _shieldHP = 3;
    [SerializeField] private ParticleSystem _trackParts;
    [SerializeField] private ParticleSystem _PowerUpPS;
    [SerializeField] private ParticleSystem _PowerDownPS;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private bool _canMove = false;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private Shoot _tankShot;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private SpawnManager _mySpawnManager;
    [SerializeField] private GameObject _tankShield;
    [SerializeField] private bool _myShieldBool = false;
    [SerializeField] private GameObject _shieldsIcon;
    [SerializeField] private GameObject _speedIcon;
    [SerializeField] private HealthManager _myHPManager;
    [SerializeField] private ParticleSystem[] _dustParts;
    [SerializeField] private UIManager _myUI;
    [SerializeField] private GameManager _myGameManager;
    [SerializeField] private Animator _camAnimator;
    [SerializeField] private MotorPitch _myEngine;
    [SerializeField] private SpeedBoostMeter _mySBM;
    [SerializeField] private SpriteRenderer _ShieldRenderer;
    [SerializeField] private Color _1hitCol, _2hitCol;
  

    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _shieldsOnClip, _shieldsOffClip;
    [SerializeField] private AudioClip _powerUpClip, _powerDownClip;
    [SerializeField] private AudioClip _slowDownClip, _shieldHitClip;
    

    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAS = GetComponent<AudioSource>();
        transform.position = new Vector2(0,-2);
        _trackParts.GetComponent<ParticleSystem>();
        _tankShot = GameObject.Find("MainTurret").GetComponent<Shoot>();
        _camAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        _myEngine = GameObject.Find("TankMotor").GetComponent<MotorPitch>();
        _mySBM = GameObject.Find("SpeedBarSprite").GetComponent<SpeedBoostMeter>();
        _currentSpeed = _speed;
    }

    void Update()
    {
        float _groundSpeed = GameManager._globalSpeed * -4f;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        ChangeVelocity(_groundSpeed);

        PlayerMovement(horizontalInput, verticalInput);
        
        if(GameManager._globalSpeed > 0)
        {

            _myAnimator.SetBool("isMoving", true);
        }
        else if(GameManager._globalSpeed == 0)
        {

            _myAnimator.SetBool("isMoving", false);
        }


    }

    private void PlayerMovement(float horizInput, float vertInput)
    {
        if (_canMove)
        {
            Vector3 direction = new Vector3(horizInput, vertInput, 0).normalized;
            _myAnimator.SetFloat("sideMove", horizInput);
            transform.Translate(direction * _speed * Time.deltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.5f, 11.5f), Mathf.Clamp(transform.position.y, -7.5f, 1.5f), 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {

                if (_speedTimer >= 0)
                {
                    if (_speedBoostActive)
                    {
                        _speedIcon.SetActive(true);
                        _myEngine.ChangeSpeed(2.5f);
                        for (int i = 0; i < _dustParts.Length; i++)
                        {
                            var main = _dustParts[i].main;
                            main.startLifetime = 1f;
                        }
                        _currentSpeed = _baseSpeed * _speedMult;
                        _speed = Mathf.MoveTowards(_speed, _currentSpeed, 1 * 5 * Time.deltaTime);
                        _speedTimer -= Time.deltaTime;
                        _mySBM.UpdateLength(_speedTimer/ _speedBoostTime);
                    }

                }
                else if (_speedTimer <= 0)
                {
                    _speedBoostActive = false;
                    _currentSpeed = _baseSpeed;
                    if(_speedTimer <= 0)
                    {
                        _PowerDownPS.Emit(15);
                        _speedTimer = 0.01f;
                        _myAS.PlayOneShot(_slowDownClip, 0.5f);
                    }
                    _speed = Mathf.MoveTowards(_speed, _currentSpeed, 1 * 5 * Time.deltaTime);
                    
                    _speedIcon.SetActive(false);
                    _myEngine.ChangeSpeed(1.25f);
                    for (int i = 0; i < _dustParts.Length; i++)
                    {
                        var main = _dustParts[i].main;
                        main.startLifetime = 0.5f;
                    }
                }
            }
 

            else
            {
                _speed = Mathf.MoveTowards(_speed, _baseSpeed, 1 * 5 * Time.deltaTime);
                _speedTimer = Mathf.MoveTowards(_speedTimer, _speedBoostTime, 1 * Time.deltaTime);
                _mySBM.UpdateLength(_speedTimer / _speedBoostTime);
                _speedIcon.SetActive(false);
                _myEngine.ChangeSpeed(1.25f);
                for (int i = 0; i < _dustParts.Length; i++)
                {
                    var main = _dustParts[i].main;
                    main.startLifetime = 0.5f;
                }
                if (_speedTimer == _speedBoostTime)
                {
                    _speedBoostActive = true;
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            _camAnimator.SetTrigger("camShake");
            OnPlayerDeath();
        }
        else if(collision.tag == "PowerUp")
        {
            _PowerUpPS.Emit(35);
            _myAS.PlayOneShot(_powerUpClip, 0.5f);
        }

    }

    public void OnPlayerDeath()
    {
        Vector2 _playerPos = transform.position;
        Instantiate(_explosion, _playerPos, Quaternion.identity);
        _myUI.GameOverMessage();
        _myGameManager.GameIsOver();
        _mySpawnManager._playerAlive = false;
        Destroy(gameObject);
    }

    public void ActivateShields()
    {
        _tankShield.SetActive(true);
        _ShieldRenderer.color = Color.white;
        _shieldsIcon.SetActive(true);
        _myShieldBool = true;
        _myAS.PlayOneShot(_shieldsOnClip, 0.35f);
        _shieldHP = 3;
    }

    private void BreakShields()
    {
        
        _myShieldBool = false;
        _PowerDownPS.Emit(15);
        StartCoroutine(TurnOffShields());

    }

    IEnumerator TurnOffShields()
    {
        yield return new WaitForSeconds(0.25f);
        _tankShield.SetActive(false);
        _shieldsIcon.SetActive(false);
        _myAS.PlayOneShot(_shieldsOffClip);
        yield break;
    }

   /* public void ActivateSpeedBoost()
    {
        //_myGameManager.SetGlobalSpeed(1f);
        //_speed *= _speedMult;
        _speedBoostActive = true;
        _speedIcon.SetActive(true);
        _myEngine.ChangeSpeed(2.5f);
        for(int i = 0; i < _dustParts.Length; i++)
        {
            var main = _dustParts[i].main;
            main.startLifetime = 1f;
        }
        //StartCoroutine(SpeedCoolDown());
    }
    */
    public void TakeDamage(int dam)
    {

        
        if (!_myShieldBool && dam > 10)
        {
            _myHPManager.UpdateHealth(dam);
            _camAnimator.SetTrigger("camShake");
        }
        else if (_myShieldBool && dam > 10)
        {
            if(_shieldHP > 1)
            {
                _shieldHP--;
                if(_shieldHP == 2)
                {
                    _ShieldRenderer.color = _1hitCol;
                }
                else if(_shieldHP == 1)
                {
                    _ShieldRenderer.color = _2hitCol;
                }
                _myAS.PlayOneShot(_shieldHitClip);
            }
            else if(_shieldHP == 1)
            {
                _shieldHP--;
                BreakShields();
            }
            
        }
        else
        {
            return;
        }
  
    }



    IEnumerator SpeedCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _myEngine.ChangeSpeed(1.25f);
        _PowerDownPS.Emit(15);
        _speed /= _speedMult;
        _speedIcon.SetActive(false);
        _myAS.PlayOneShot(_powerDownClip);
        for (int i = 0; i < _dustParts.Length; i++)
        {
            var main = _dustParts[i].main;
            main.startLifetime = 0.5f;
        }
    }
   
    
    private void ChangeVelocity(float speed)
    {
        var _myLiveParts = new ParticleSystem.Particle[_trackParts.main.maxParticles];


        int aliveParticlesCount = _trackParts.GetParticles(_myLiveParts);

        for (int i = 0; i < aliveParticlesCount; i++)
        {
            _myLiveParts[i].velocity = new Vector3(0, speed, 0);
        }
        _trackParts.SetParticles(_myLiveParts, aliveParticlesCount);
    }

    public void AllowMovement()
    {
        _canMove = true;
        _tankShot.ActivateWeapons();
    }

    public void DisableMovement()
    {
        _canMove = false;
        _tankShot.ActivateWeapons();
    }

    public void PowerDownAudio()
    {
        _myAS.PlayOneShot(_powerDownClip, 0.5f);
    }

}
