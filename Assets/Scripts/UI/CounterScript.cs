using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour
{

    [SerializeField] private Material[] _matList;
    [SerializeField] private Vector2[] _currentOffset, _nextOffset;
    [SerializeField] private float[] _step;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _inputPoints;
    [SerializeField] private int _totalPoints = 0;
    [SerializeField] private decimal[] _cogNumbers = new decimal[] {0,0,0,0,0,0,0};
    [SerializeField] private float[] _cogSpeed;
    [SerializeField] private int _maxPoints;


    void Start()
    {
        _totalPoints = 0;
        for( int i = 0; i < _matList.Length; i++ )
        {
           _matList[i].mainTextureOffset = new Vector2(0, 0);  
        }
    }


    void Update()
    {
        
        for(int i = 0; i < _matList.Length; i++)
        {
            _currentOffset[i] = _matList[i].mainTextureOffset;

        }

        for (int i = 0; i < _matList.Length; i++)
        {
            if (_currentOffset[i].y != _nextOffset[i].y)
            {
                _matList[i].mainTextureOffset = Vector2.MoveTowards(_currentOffset[i], _nextOffset[i], _speed * (_inputPoints * _cogSpeed[i])* Time.deltaTime);
            }
        }

    }


    public void PointsToCounter(int _points)
    {
        
        _totalPoints += _points;
        if(_totalPoints > _maxPoints -1)
        {
            _totalPoints = _maxPoints;
        }
        _inputPoints = _points;
        Debug.Log("Total Score: " + _totalPoints);
 

        for (int n = 0; n < _matList.Length; n++)
        {
  
            _cogNumbers[n] = _totalPoints * (decimal)_step[n];
            _cogNumbers[n] = System.Math.Truncate(_cogNumbers[n]);
            _cogNumbers[n] = _cogNumbers[n] * (decimal)0.1f;
            _nextOffset[n] = new Vector2(0, (float)-_cogNumbers[n]);

            //Debug.Log("Number " + n + ": " + _cogNumbers[n]);
        }

    }
}
