using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterParent : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _targetPosX;
    [SerializeField] private Vector2 _myPos;
    [SerializeField] private bool _dodging = false;
    [SerializeField] private bool _dead = false;

    void Start()
    {
        _myPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_dead)
        {
            _speed = GameManager._globalSpeed * 6f;
            if(transform.position.y < -12f)
            {
                transform.position = new Vector2(Random.Range(-11.5f, 11.5f), 12f);
            }
        }
        else if(_dead)
        {
            _speed = GameManager._globalSpeed * 4f;
        }

        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        _myPos = transform.position;
        if(transform.position.x != _targetPosX && _dodging)
        {
            transform.position = Vector2.MoveTowards(_myPos, new Vector2(_targetPosX, _myPos.y), 1 * _speed * Time.deltaTime);
        }
        else
        {
            _dodging = false;
        }
        
        
    }

    public void DodgeObstacle(float xDir)
    {
        if(_dodging == false)
        {
            _targetPosX = transform.position.x + xDir;
            _dodging = true;
        }

    }

    public void OnDeath()
    {
        _dead = true;
    }
}
