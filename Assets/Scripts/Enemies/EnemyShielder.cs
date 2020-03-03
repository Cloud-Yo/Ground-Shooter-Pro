using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShielder : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _pointsValue = 75;
    [SerializeField] private int _damAmount = 65;
    [SerializeField] private int _laserDam = 15;
    [SerializeField] private int _shieldStrength = 1;
    [SerializeField] private int _hitSparkCount = 0;
    [SerializeField] private Vector2 _laserHitPoint;
    [SerializeField] private Vector2 _laserTempPoint;

    [SerializeField] private Collider2D _myShieldCol;
    [SerializeField] private Collider2D _myBodyCol;
    [SerializeField] private LayerMask _obstacleLayerMask, _playerLayerMask;

    [SerializeField] private Animator _myAnim;
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _enemyPopClip, _ShieldBreakClip, _laserFireClip, _laserHitClip;
    [SerializeField] private ParticleSystem _myPS;
    [SerializeField] private ParticleSystem _laserPS;
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private CounterScript _myCounter;
    [SerializeField] private UIManager _myUI;
    [SerializeField] private LineRenderer _laserLineR;
    [SerializeField] private GameObject _laserHolder;
    [SerializeField] private GameObject _laserHitPS;

    [SerializeField] private bool _dead;
    [SerializeField] private bool _shieldBroken;
    [SerializeField] private bool _fireLaser = false;
    [SerializeField] private bool _canFire = false;
    [SerializeField] private bool _resetPos = false;


    void Start()
    {
        _playerGO = GameObject.Find("Player");
        if (_playerGO == null)
        {
            Debug.LogError(transform.name + ": _PlayerGO is NULL.");
            _playerGO = null;
        }

        _myCounter = GameObject.Find("ScoreCounter").GetComponent<CounterScript>();
        _myUI = GameObject.Find("UIManager").GetComponent<UIManager>();
        _laserLineR.enabled = false;
        _laserLineR.SetPosition(0, Vector3.zero);
        _laserLineR.SetPosition(1, Vector3.zero);
        StartCoroutine(FireLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_dead)
        {
            _speed = GameManager._globalSpeed * 6f;
        }
        else if (_dead)
        {
            _speed = GameManager._globalSpeed * 4f;
            _canFire = false;
            StopAllCoroutines();
        }

        transform.Translate(Vector2.down * _speed * Time.deltaTime);
       
        if (transform.position.y <= -8f && _dead != true)
        {
            //transform.position = new Vector2(Random.Range(-11.5f, 11.5f), 8.5f);
            Destroy(this.gameObject);
            _myUI.EnemyMissedScore();
        }
        else if (transform.position.y <= -12.5f && _dead == true)
        {
            Destroy(this.gameObject);
        }
        DodgeObstacle();

        _laserLineR.SetPosition(0, _laserHolder.transform.position);
        FireLaser();

    }

    private void DodgeObstacle()
    {
        RaycastHit2D boxCaster = Physics2D.BoxCast(_myBodyCol.bounds.center, _myBodyCol.bounds.size, 0f, Vector2.down, 3f, _obstacleLayerMask);
        Color rayColor;


        if (boxCaster.collider != null)
        {
            rayColor = Color.green;

            Collider2D _col = boxCaster.collider;
            Vector2 _colCenter = _col.bounds.center;

            if (transform.position.x > _colCenter.x)
            {
                transform.position = new Vector2(transform.position.x + 0.05f, transform.position.y);
            }
            else if (transform.position.x <= _colCenter.x)
            {
                transform.position = new Vector2(transform.position.x - 0.05f, transform.position.y);
            }

        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(_myBodyCol.bounds.center, Vector2.down, rayColor);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy" && _dead == false)
        {
            Collider2D _col = collision.transform.GetComponent<Collider2D>();
            Vector2 _colCenter = _col.bounds.center;

            if (transform.position.x > _colCenter.x)
            {
                transform.position = new Vector2(transform.position.x + 0.05f, transform.position.y);
            }
            else if (transform.position.x <= _colCenter.x)
            {
                transform.position = new Vector2(transform.position.x - 0.05f, transform.position.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" || collision.tag == "Explosion")
        {

            if (transform.tag == "Enemy")
            {
                if(_shieldStrength > 0 && !_shieldBroken)
                {
                    _shieldStrength--;
                    if(_shieldStrength == 0)
                    {
                        _myShieldCol.enabled = false;
                        _myAnim.SetBool("breakShield", true);
                        _myAS.PlayOneShot(_ShieldBreakClip, 0.8f);
                        _shieldBroken = true;
                    }
           
                }
                else if(_shieldStrength == 0 && _shieldBroken && !_dead)
                {
                    transform.tag = "Untagged";
                    _dead = true;
                    _laserHolder.SetActive(false);
                    _myBodyCol.enabled = false;
                    _myAnim.SetBool("Death", true);
                    _myPS.Play();
                    GetComponent<SpriteRenderer>().sortingOrder = 0;
                    _myAS.PlayOneShot(_enemyPopClip, 0.5f);
                    _myCounter.PointsToCounter(_pointsValue);
                    _myUI.EnemyKillScore();
                }


            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if(!_dead)
            {
 
                _dead = true;
                _myShieldCol.enabled = false;
                _myBodyCol.enabled = false;
                _laserHolder.SetActive(false);
                if(!_shieldBroken)
                {
                    _myAnim.SetBool("PlayerCol", true);
                    _myAS.PlayOneShot(_ShieldBreakClip, 0.8f);
                }
                else if(_shieldBroken)
                {
                    _myAnim.SetBool("Death", true);
                }

                _myAS.PlayOneShot(_enemyPopClip, 0.5f);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                _myPS.Play();
                _myCounter.PointsToCounter(_pointsValue);
                collision.gameObject.GetComponent<Player>().TakeDamage(_damAmount);
                _myUI.EnemyKillScore();

            }
            
        }
    }

   
    private void FireLaser()
    {
        RaycastHit2D _laserHit = Physics2D.Raycast(_laserHolder.transform.position, Vector2.down, 10f, _playerLayerMask);

        if (_canFire)
        {

            if (_laserHit.collider != null && _canFire)
            {
                   if (_laserHit.collider.CompareTag("Player"))
                   {
                    _laserLineR.enabled = true;
                    _fireLaser = true;
                    _laserHitPoint = _laserHit.point;
                    if(!_resetPos)
                    {
                        StartCoroutine(LaserHitRoutine());
                        _resetPos = true;
                    }
           


                    if (_fireLaser)
                    {
                        if (!_laserPS.isEmitting)
                        {
                            _laserPS.Emit(3);
                        }
                        Vector2 _currEndPos = _laserLineR.GetPosition(1);
                        _laserLineR.SetPosition(1, Vector3.MoveTowards(_currEndPos, _laserHitPoint, 8 * _speed * Time.deltaTime));
                        _currEndPos = _laserLineR.GetPosition(1);
                        if (_currEndPos == _laserHitPoint)
                        {
                            if (_laserHit.collider != null)
                            {

                                Debug.Log("Laser hit Player!");
                                if(_hitSparkCount < 1)
                                {
                                    Instantiate(_laserHitPS, _laserHit.point, Quaternion.identity);
                                    _myAS.PlayOneShot(_laserHitClip, 0.5f);
                                    _hitSparkCount++;
                                }

                                _laserHit.collider.GetComponent<Player>().TakeDamage(_laserDam);
                                _canFire = false;
                                _laserLineR.enabled = false;


                            }

                        }
                    }

                }
            }
            else if(_laserHit.collider == null)
            {
                _fireLaser = false;
                _laserLineR.enabled = false;
                _canFire = false;
                _resetPos = false;
                _hitSparkCount = 0;
            }



        }
        
       
    }
    
    IEnumerator FireLaserRoutine()
    {
        while(!_dead)
        {
            yield return new WaitForSeconds(3f);
            if(!_canFire)
            {
                _canFire = true;
            }
        }

    }

    IEnumerator LaserHitRoutine()
    {
        
        _laserLineR.SetPosition(1, _laserLineR.GetPosition(0));
        _myAS.PlayOneShot(_laserFireClip, 0.5f);
        yield return new WaitForSeconds(0.75f);
        
        _fireLaser = false;
        _laserLineR.enabled = false;
        _canFire = false;
        _resetPos = false;
        _hitSparkCount = 0;
        yield break;
    }
}
