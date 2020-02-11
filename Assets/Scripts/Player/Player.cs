using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMult = 2f;
    [SerializeField] private ParticleSystem _trackParts;
    [SerializeField] private ParticleSystem _PowerUpPS;
    [SerializeField] private ParticleSystem _PowerDownPS;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private bool _canMove = false;
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

    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _shieldsOnClip, _shieldsOffClip;
    [SerializeField] private AudioClip _powerUpClip, _powerDownClip;
    

    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAS = GetComponent<AudioSource>();
        transform.position = new Vector2(0,-2);
        _trackParts.GetComponent<ParticleSystem>();
        _tankShot = GameObject.Find("MainTurret").GetComponent<Shoot>();
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
            _myAnimator.SetFloat("sideMove", horizInput);
            transform.Translate(new Vector3(horizInput, vertInput, 0) * _speed * Time.deltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.5f, 11.5f), Mathf.Clamp(transform.position.y, -7.5f, 1.5f), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
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
        _shieldsIcon.SetActive(true);
        _myShieldBool = true;
        _myAS.PlayOneShot(_shieldsOnClip, 0.35f);
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

    public void ActivateSpeedBoost()
    {
        _myGameManager.SetGlobalSpeed(1f);
        _speed *= _speedMult;
        _speedIcon.SetActive(true);
        for(int i = 0; i < _dustParts.Length; i++)
        {
            var main = _dustParts[i].main;
            main.startLifetime = 1f;
        }
        StartCoroutine(SpeedCoolDown());
    }

    public void TakeDamage(int dam)
    {
        if(!_myShieldBool)
        {
            _myHPManager.UpdateHealth(dam);
        }
        else if (_myShieldBool && dam > 10)
        {
            BreakShields();
        }
        else
        {
            return;
        }

            
    }

    IEnumerator SpeedCoolDown()
    {
        yield return new WaitForSeconds(5f);
        _myGameManager.SetGlobalSpeed(0.5f);
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
