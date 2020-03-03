using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyRunner : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private CounterScript _myCounter;
    [SerializeField] private int _pointsValue;
    [SerializeField] private bool _dead = false;
    [SerializeField] private bool didFire = false;
    [SerializeField] private bool _bulletSeen = false;
    [SerializeField] private int _damAmount = 35;
    [SerializeField] private float _jumpTargetX = 0;
    [SerializeField] private UIManager _uiMan;
    [SerializeField] private Player _player;
    
    [SerializeField] private GameObject _enemyBullet;
    [SerializeField] private int _isEnemyShooter = 0;


    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _EnemyPopClip, _enemySpitClip;
 

    [SerializeField] private Collider2D myCollider;
    [SerializeField] private LayerMask obstacleLayerMask, powerUpLayerMask, _bulletLayerMask;



    void Start()
    {
        _isEnemyShooter = Random.Range(0, 2);
        switch(_isEnemyShooter)
        {
            case 0:
                break;
            case 1:
                StartCoroutine(EnemyShootingRoutine());
                break;
        }
        
        _myCounter = GameObject.Find("ScoreCounter").GetComponent<CounterScript>();
        _uiMan = GameObject.Find("UIManager").GetComponent<UIManager>();
        if(GameObject.Find("Player") != null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }
        else
        {
            _player = null;
        }
        _jumpTargetX = 0;


}


    void Update()
    {
        if(!_dead)
        {
            _speed = GameManager._globalSpeed * 8f;
            transform.Translate(new Vector2(_jumpTargetX, -1) * _speed * Time.deltaTime);
        }
        else if(_dead)
        {
            _speed = GameManager._globalSpeed * 4f;
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }
        
     
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -11.5f, 11.5f), transform.position.y);
        if(transform.position.y <= -8f && _dead != true)
        {
            
            transform.position = new Vector2(Random.Range(-11.5f, 11.5f), 8.5f);
            _uiMan.EnemyMissedScore();
            didFire = false;
            
                    
        }
        else if(transform.position.y <= -12.5f && _dead == true)
        {
            Destroy(this.gameObject);
        }
        DodgeObstacle();
        DestroyPowerup();


        if (_player == null)
        {
            _player = null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet" || collision.tag == "Explosion")
        {

            if(transform.tag == "Enemy")
            {
                transform.tag = "Untagged";
                _dead = true;
                StopCoroutine(EnemyShootingRoutine());
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Animator>().SetBool("death", true);
                GetComponent<ParticleSystem>().Play();
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                _myAS.PlayOneShot(_EnemyPopClip);
                _myCounter.PointsToCounter(_pointsValue);
                _uiMan.EnemyKillScore();
            
            }
  

        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            
            GetComponent<Collider2D>().enabled = false;
            _dead = true;
            StopCoroutine(EnemyShootingRoutine());
            GetComponent<Animator>().SetBool("death", true);
            GetComponent<ParticleSystem>().Play();
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            _myAS.PlayOneShot(_EnemyPopClip);
            _myCounter.PointsToCounter(_pointsValue);
            collision.gameObject.GetComponent<Player>().TakeDamage(_damAmount);
            _uiMan.EnemyKillScore();

        }

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



    private void DodgeObstacle()
    {
        RaycastHit2D boxCaster = Physics2D.BoxCast(myCollider.bounds.center,myCollider.bounds.size, 0f, Vector2.down, 3f, obstacleLayerMask);
        RaycastHit2D bulletSense = Physics2D.BoxCast(myCollider.bounds.center, new Vector2(.5f, 1f), 0f, Vector2.down, 5f, _bulletLayerMask);
        Color rayColor;
        bool _bulletSeen = false;

        if(boxCaster.collider != null)
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

        if (bulletSense.collider != null && !_dead && !_bulletSeen)
        {
            rayColor = Color.blue;

            Collider2D _col = bulletSense.collider;
            Vector2 _myCenter = _col.bounds.center;
            int myDir = 0;
            int Jump = Random.Range(0, 3);
            _bulletSeen = true;
            if (transform.position.x > _myCenter.x)
            {
                myDir = 1;
            }
            else if (transform.position.x <= _myCenter.x)
            {
                myDir = -1;
            }
            StartCoroutine(AvoidBulletRoutine(_bulletSeen, myDir, Jump));

        }

        Debug.DrawRay(myCollider.bounds.center, Vector2.down, rayColor);
    }

    private void DestroyPowerup()
    {
        RaycastHit2D _myRay = Physics2D.Raycast(myCollider.bounds.center, Vector2.down, 5f, powerUpLayerMask);
        
        if(_myRay.collider != null)
        {
            
            if (_dead == false && !didFire)
            {
                Instantiate(_enemyBullet, transform.position, Quaternion.identity);
                _myAS.PlayOneShot(_enemySpitClip, 0.5f);
                didFire = true;

            }
        }
    }

    IEnumerator EnemyShootingRoutine()
    {

        yield return new WaitForSeconds(Random.Range(2f, 5f));
        if (_dead == false)
        {
            Instantiate(_enemyBullet, transform.position, Quaternion.identity);
            _myAS.PlayOneShot(_enemySpitClip, 0.5f);
        }
 

    }

    IEnumerator AvoidBulletRoutine(bool detected, int dir, int jumper)
    {


        if (jumper == 0 && detected == true)
        {

            switch (dir)
            {
                case 0:
                    _jumpTargetX = 1;
                    break;
                case 1:
                    _jumpTargetX = -1;
                    break;
            }
            yield return new WaitForSeconds(0.5f);
            _bulletSeen = false;
            detected = false;
            _jumpTargetX = 0;
        }
        else if(jumper != 0 && detected == true)
        {
 
            yield return new WaitForSeconds(0.5f);
            _bulletSeen = false;
            detected = false;
            _jumpTargetX = 0;
        }

    }
}
