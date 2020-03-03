using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRammer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _ramDist = 5.5f;
    [SerializeField] private float _ramSpeed = 2.5f;
    [SerializeField] private float _jumpTargetX = 0f;
    [SerializeField] private bool _dead;
    [SerializeField] private bool _bulletSeen = false;
    [SerializeField] private bool _irRamming = false;
    [SerializeField] private Vector2 _vel;
    [SerializeField] private int _pointsValue = 45;
    [SerializeField] private int _damAmount = 55;

    [SerializeField] private Collider2D _myCollider;
    [SerializeField] private LayerMask _obstacleLayerMask, _bulletLayerMask;

    [SerializeField] private GameObject _playerGO;
    [SerializeField] private CounterScript _myCounter;
    [SerializeField] private UIManager _myUI;

    [SerializeField] private Animator _myAnim;
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _explodeClip, _enemyHowlClip;




    void Start()
    {
        _playerGO = GameObject.Find("Player");
        if(_playerGO == null)
        {
            Debug.LogError(transform.name + ": _PlayerGO is NULL.");
            _playerGO = null;
        }

        _myCounter = GameObject.Find("ScoreCounter").GetComponent<CounterScript>();
        _myAS.PlayOneShot(_enemyHowlClip, 0.85f);
        _myUI = GameObject.Find("UIManager").GetComponent<UIManager>();
        _jumpTargetX = 0;
    }


    void Update()
    {

        if (!_dead)
        {
            _speed = GameManager._globalSpeed * 9f;
            transform.Translate(new Vector2(_jumpTargetX, -1) * _speed * Time.deltaTime);
        }
        else if (_dead)
        {
            _speed = GameManager._globalSpeed * 4f;
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }


        if(_playerGO != null)
        {
            if(!_dead)
            {
                if ((_playerGO.transform.position - transform.position).magnitude < _ramDist && transform.position.y > _playerGO.transform.position.y)
                {

                    if(!_irRamming)
                    {
                        _myAS.PlayOneShot(_enemyHowlClip, .75f);
                        _irRamming = true;
                    }
                    transform.position = Vector2.SmoothDamp(transform.position, new Vector2(_playerGO.transform.position.x, transform.position.y), ref _vel, _ramSpeed * Time.deltaTime, _ramSpeed);


                }
            }

        }

        if (transform.position.y <= -8f && _dead != true)
        {
            transform.position = new Vector2(Random.Range(-11.5f, 11.5f), 8.5f);
            _myUI.EnemyMissedScore();
        }
        else if (transform.position.y <= -12.5f && _dead == true)
        {
            Destroy(this.gameObject);
        }
        DodgeObstacle();

    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _dead = true;
            transform.tag = "Untagged";
            GetComponent<Collider2D>().enabled = false;
            _myAnim.SetBool("dead", true);
            GetComponent<ParticleSystem>().Play();
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            _myAS.PlayOneShot(_explodeClip, 0.5f);
            _myCounter.PointsToCounter(_pointsValue);
            collision.gameObject.GetComponent<Player>().TakeDamage(_damAmount);
            _myUI.EnemyKillScore();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" || collision.tag == "Explosion")
        {

            if (transform.tag == "Enemy")
            {
                transform.tag = "Untagged";
                _dead = true;
                GetComponent<Collider2D>().enabled = false;
                _myAnim.SetBool("dead", true);
                GetComponent<ParticleSystem>().Play();
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                _myAS.PlayOneShot(_explodeClip, 0.5f);
                _myCounter.PointsToCounter(_pointsValue);
                _myUI.EnemyKillScore();

            }

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
        RaycastHit2D boxCaster = Physics2D.BoxCast(_myCollider.bounds.center, _myCollider.bounds.size, 0f, Vector2.down, 3f, _obstacleLayerMask);
        RaycastHit2D bulletSense = Physics2D.BoxCast(_myCollider.bounds.center, new Vector2(.2f, 1f), 0f, Vector2.down, 5f, _bulletLayerMask);
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
        Debug.DrawRay(_myCollider.bounds.center, Vector2.down, rayColor);

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
        else if (jumper != 0 && detected == true)
        {

            yield return new WaitForSeconds(0.5f);
            _bulletSeen = false;
            detected = false;
            _jumpTargetX = 0;
        }

    }
}
