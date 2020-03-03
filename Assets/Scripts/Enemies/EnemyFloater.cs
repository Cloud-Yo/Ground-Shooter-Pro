using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloater : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _sinAmplitude = 1f;
    [SerializeField] private float _sinFreq;
    [SerializeField] private CounterScript _myCounter;
    [SerializeField] private int _pointsValue = 65;
    [SerializeField] private bool _dead = false;
    [SerializeField] private bool _startedShooting = false;
    [SerializeField] private int _damAmount = 35;


    [SerializeField] private GameObject _myShadow;
    [SerializeField] private GameObject _myBullet;
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private UIManager _myUI;

    [SerializeField] private Animator _myAnim;
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _EnemyPopClip;
    [SerializeField] private AudioClip _enemySpitClip;



    [SerializeField] private Collider2D _myCollider;
    [SerializeField] private LayerMask obstacleLayerMask, powerUpLayerMask;

    void Start()
    {

        _sinFreq = Random.Range(1f, 5f);
        _sinAmplitude = Random.Range(-0.75f, -1.25f);
        _playerGO = GameObject.Find("Player");
        if(_playerGO == null)
        {
            Debug.LogError(transform.name + ": PlayerGO is NULL.");
            _playerGO = null;
        }
        _myUI = GameObject.Find("UIManager").GetComponent<UIManager>();
        _myCounter = GameObject.Find("ScoreCounter").GetComponent<CounterScript>();



    }

    // Update is called once per frame
    void Update()
    {

        if (!_dead)
        {

            if (transform.position.y < -12.5f)
            {
                _myUI.EnemyMissedScore();
               
            }
            else if(transform.position.y > 11.5f)
            {
                StopCoroutine(ShootBackAtPlayer(0));
                _startedShooting = false;
            }
        }
        else if (_dead)
        {
            if(transform.position.y < -12.0f)
            {
                if(transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        if(GameManager._globalSpeed > 0)
        {
            DodgeObstacle();
            FloaterMove();
        }
        
        


        if (_playerGO != null)
        {
            if (transform.position.y < _playerGO.transform.position.y && (transform.position - _playerGO.transform.position).magnitude > 2 && !_startedShooting && !_dead)
            {

                StartCoroutine(ShootBackAtPlayer(3));
                _startedShooting = true;
            }
        }

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -12.5f, 12.5f), transform.position.y);
    }

    private void FloaterMove()
    {
        if(!_dead)
        {
            float x = (transform.parent.transform.position.x + Mathf.Sin(Time.time * _sinFreq) * _sinAmplitude);
            float y = transform.parent.position.y;

            transform.position = new Vector2(x, y);
        }
        
    }


    private void DodgeObstacle()
    {
        RaycastHit2D boxCaster = Physics2D.BoxCast(_myCollider.bounds.center, _myCollider.bounds.size * 1.5f, 0f, Vector2.down, 3f, obstacleLayerMask);
        Color rayColor;

        if (boxCaster.collider != null)
        {
            rayColor = Color.green;

            Collider2D _col = boxCaster.collider;
            Vector2 _colCenter = _col.bounds.center;

            if (transform.parent.transform.position.x > _colCenter.x)
            {
                float xDir = _sinAmplitude * -1.5f;
                GetComponentInParent<FloaterParent>().DodgeObstacle(xDir);

            }
            else if (transform.parent.transform.position.x <= _colCenter.x)
            {
                float xDir = _sinAmplitude * 1.5f;
                GetComponentInParent<FloaterParent>().DodgeObstacle(xDir);
            }

        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(_myCollider.bounds.center, Vector2.down, rayColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Explosion"))
        {

            if (transform.CompareTag("Enemy"))
            {
                
                _dead = true;
                GetComponentInParent<FloaterParent>().OnDeath();
                transform.tag = "Untagged";
                _myShadow.SetActive(false);
                _myCollider.enabled = false;
                _myAnim.SetBool("Pop", true);
                GetComponent<ParticleSystem>().Emit(25);
                GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                _myAS.PlayOneShot(_EnemyPopClip);
                _myCounter.PointsToCounter(_pointsValue);
                _myUI.EnemyKillScore();

            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            _dead = true;
            GetComponentInParent<FloaterParent>().OnDeath();
            transform.tag = "Untagged";
            Destroy(_myShadow.gameObject);
            _myCollider.enabled = false;
            _myAnim.SetBool("Pop", true);
            GetComponent<ParticleSystem>().Emit(25);
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            _myAS.PlayOneShot(_EnemyPopClip);
            _myCounter.PointsToCounter(_pointsValue);
            collision.gameObject.GetComponent<Player>().TakeDamage(_damAmount);
            _myUI.EnemyKillScore();
         }
    }



    private IEnumerator ShootBackAtPlayer(int bullets)
    {
        
       if(!_dead)
        {

            while(bullets > 0)
            {
                yield return new WaitForSeconds(0.5f);
                Vector2 shootAngle;
                if (_playerGO != null)
                {
                    shootAngle = _playerGO.transform.position - this.transform.position;
                }
                else 
                {
                    shootAngle = Vector2.up;
                }
                
                Instantiate(_myBullet, transform.position, Quaternion.FromToRotation(Vector2.down, shootAngle));
                _myAS.PlayOneShot(_enemySpitClip, 0.5f);
                bullets--;

             }
            

        }

    }

}
