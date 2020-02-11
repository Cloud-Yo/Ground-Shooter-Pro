using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCog : MonoBehaviour
{

    //[SerializeField] private Material _numTexture;
    [SerializeField] private Material _cogTexTure;
    [SerializeField] private Vector2 _currentOffset;
    [SerializeField] private Vector2 _startOffset;
    [SerializeField] private Vector2 _nextOffset;
    [SerializeField] private float _speed = 1f;
    [SerializeField] public int _myValue = 1;
    [SerializeField] private int _currentValue;
    [SerializeField] private Vector2 _rate = new Vector2(0f, 0.1f);
    //[SerializeField] private CounterCog _nextCounterCog;
     
    // Start is called before the first frame update
    void Start()
    {
        _cogTexTure = GetComponent<SpriteRenderer>().sharedMaterial;
        _currentValue = 0;
        _nextOffset = new Vector2(0,0);
        _currentOffset = new Vector2(0,0);
        _cogTexTure.mainTextureOffset = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        //Material matHolder = _numTexture;
        //Material cogHolder = _cogTexTure;
        //_currentOffset = matHolder.mainTextureOffset;
   
        
        
        if(Input.GetKeyDown(KeyCode.O))
        {
            _startOffset = _nextOffset;
            _nextOffset = _currentOffset - (_rate * _myValue);
            _currentValue += _myValue;
            if(_currentValue > 9)
            {
                _currentValue = _currentValue - 9;
                
            }
           
        }

        if (_currentOffset.y > _nextOffset.y)
        {
            _cogTexTure.mainTextureOffset = Vector2.MoveTowards(_currentOffset, _nextOffset, (_speed * _myValue) * Time.deltaTime);
            //cogHolder.mainTextureOffset = Vector2.MoveTowards(_currentOffset, _nextOffset, (_speed * _myValue) * Time.deltaTime);
            _currentOffset = _cogTexTure.mainTextureOffset;
        }    
    }

    public void AddPoints(int _points)
    {
        _startOffset = _nextOffset;
        _nextOffset = _currentOffset - (_rate * _points);
        _currentValue += _points;
        if (_currentValue > 9)
        {
            //_nextCounterCog._myValue = _currentValue - 9;
            _currentValue = 0;
        }

    }


        

         
        
  
}
