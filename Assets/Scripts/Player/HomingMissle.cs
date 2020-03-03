using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HomingMissle : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotSpeed = 150;
    [SerializeField] private GameObject _myExplosion;
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private AudioClip _rocketHissClip;
    [SerializeField] private GameObject[] _Targets;
    [SerializeField] private float[] _targetDistances;
    [SerializeField] private GameObject _myTarget;
    [SerializeField] private Rigidbody2D _myRB;
    [SerializeField] private SpriteRenderer _mySR;
    [SerializeField] private bool _homing = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _targetDistances = new float[_Targets.Length];
        
        
        for (int i = 0; i < _Targets.Length; i++)
        {
            float _enemyDist = (_Targets[i].transform.position - this.transform.position).magnitude;
            _targetDistances[i] = _enemyDist;
            
        }
    }

    void Start()
    {
        _mySR = GetComponent<SpriteRenderer>();
        _myAS.clip = _rocketHissClip;
        _myAS.Play();
        int _newTarget = Array.IndexOf(_targetDistances, Mathf.Min(_targetDistances));
        if(_Targets.Length > 0 && _Targets[_newTarget].transform.tag == "Enemy")
        {
            _myTarget = _Targets[_newTarget];
        }
        else
        {
            _myTarget = null;
            StartCoroutine(DestroyMissle());
        }

        StartCoroutine(ActivateHoming());
        _mySR.sortingOrder = 13;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction;

        if (_myTarget != null)
        {
            direction = _myTarget.transform.position - this.transform.position;
            direction.Normalize();
        }
        else
        {

            FindTarget();
            direction = Vector2.up;
            _homing = false;
            
        }
        

        

        if(_homing)
        {
            float rotDirection = Vector3.Cross(direction, transform.up).z;

            _myRB.angularVelocity = -rotDirection * _rotSpeed;
           
        }
        transform.Translate(Vector2.up * _speed * Time.deltaTime, Space.Self);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Obstacle")
        {
            ExplodeRocket();
        }
    }

    public void ExplodeRocket()
    {
        GameObject explosion = Instantiate(_myExplosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator DestroyMissle()
    {
        yield return new WaitForSeconds(1f);
        if(_myTarget == null)
        {
            ExplodeRocket();
        }
  
    }

    IEnumerator ActivateHoming()
    {
        yield return new WaitForSeconds(0.25f);
        _homing = true;
        yield break;
    }

    private void FindTarget()
    {
        _Targets = GameObject.FindGameObjectsWithTag("Enemy");
        if(_Targets == null)
        {
            DestroyMissle();
        }
        _targetDistances = new float[_Targets.Length];


        for (int i = 0; i < _Targets.Length; i++)
        {
            float _enemyDist = (_Targets[i].transform.position - this.transform.position).magnitude;
            _targetDistances[i] = _enemyDist;

        }

        int _newTarget = Array.IndexOf(_targetDistances, Mathf.Min(_targetDistances));

        if (_Targets.Length > 0 && _Targets[_newTarget].transform.tag == "Enemy")
        {
            _myTarget = _Targets[_newTarget];
        }
        else
        {
            _myTarget = null;

        }

    }

}
