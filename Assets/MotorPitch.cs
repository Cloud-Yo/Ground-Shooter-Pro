using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorPitch : MonoBehaviour
{
    [SerializeField] private AudioSource _myAS;
    [SerializeField] private float _startSpeed = .8f;
   [SerializeField] private float _currentSpeed = 0;
    [SerializeField] private float _newSpeed;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _pitchMod;
    

    void Start()
    {
        _myAS = GetComponent<AudioSource>();
        _myAS.pitch = _startSpeed;
        _currentSpeed = _startSpeed;
    }

    void Update()
    {
        _pitchMod = Input.GetAxis("Vertical") * .15f;
        _newSpeed = (GameManager._globalSpeed * 2.5f) + _pitchMod;
        

        
        if(GameManager._globalSpeed > 0)
        {
            _myAS.pitch = Mathf.MoveTowards(_currentSpeed, _newSpeed, _speed * Time.deltaTime);
            _currentSpeed = _myAS.pitch;
        }
        

         
    }
}
