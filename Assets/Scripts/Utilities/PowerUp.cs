using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _speedMult = 3f;
    [SerializeField] private int _powerUpID;
    [SerializeField] private GameObject[] _powerUpImg;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private bool _attracted = false;
    [SerializeField] private Vector2 _vel;

    [SerializeField] private HealthManager _playerHP;
    [SerializeField] private Shoot _playerGun;
    [SerializeField] private Player _player;
  
     
    // Start is called before the first frame update
    void Start()
    {
        _playerHP = GameObject.Find("TankUIPanel").GetComponent<HealthManager>();
        if(_playerHP == null)
        {
            Debug.LogError("PowerUp: _PlayerHP is NULL");
        }

        _playerGun = GameObject.Find("MainTurret").GetComponent<Shoot>();
        if(_playerGun == null)
        {
            Debug.LogError("PowerUp:  _playerGun is NULL.");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {

        _speed = GameManager._globalSpeed * 4f;
        if (!_attracted)
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime, Space.World);
        }
        else if(_attracted)
        {
            
            transform.position = Vector2.SmoothDamp(transform.position, _playerGun.transform.position, ref _vel, _speed * Time.deltaTime, _speed * _speedMult);
        }


        if(transform.position.y <= -10f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
            switch(_powerUpID)
            {
                case 0: //TripleShot
                    collision.GetComponentInChildren<Shoot>().ActivateTripleShot();
                    Destroy(this.gameObject);
                    break;
                case 1: //Shields
                    collision.GetComponent<Player>().ActivateShields();
                    Destroy(this.gameObject);
                    break;
                case 2: //Ammo
                    _playerGun.ReloadAmmo(15);
                    Destroy(this.gameObject);
                    break;
                case 3: //Ammo
                    _playerGun.ReloadAmmo(15);
                    Destroy(this.gameObject);
                    break;
                case 4: //+1 life
                    _playerHP.AddLives();
                    Destroy(this.gameObject);
                    break;
                case 5: //GrapeShot
                    _playerGun.ActivateGrapeShot();
                    Destroy(this.gameObject);
                    break;
                case 6: //Homing Missle
                    _playerGun.ActivateHomingMissle();
                    Destroy(this.gameObject);
                    break;
                case 7: //mimic powerUp
                    Instantiate(_explosion, transform.position, Quaternion.identity);
                    _player.MimicAttack(50);
                    Destroy(this.gameObject);
                    break;

            }
           


        }

        else if(collision.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }

        else if(this.transform.CompareTag("Enemy") && collision.CompareTag("Bullet"))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void PowerUpType(int _type)
    {
        _powerUpID = _type;
        _powerUpImg[_powerUpID].SetActive(true);
    }

    public void AttractedState()
    {
        _attracted = true;
    }

    public void NotAttractedState()
    {
        _attracted = false;
    }

    public void BecomeEnemy()
    {
        this.transform.tag = "Enemy";
    }
}
