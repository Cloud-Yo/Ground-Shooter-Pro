using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeed : MonoBehaviour
{
    [SerializeField] private ParticleSystem _myPS;
    [SerializeField] private float _speed;
    private ParticleSystem[] _myParts;
    // Start is called before the first frame update
    void Start()
    {
        _myPS = GetComponent<ParticleSystem>();
        if(_myPS == null)
        {
            Debug.Log("ParticleSystem is NULL.");
        }
     

    }

    // Update is called once per frame
    void Update()
    {
        _speed = GameManager._globalSpeed * -4f;
        var main = _myPS.main;
  

        if (_speed != 0 && _myPS.isPaused)
        {

           
            _myPS.Play();
            main.startSpeed = _speed;

        }
        else if(_speed == 0 && _myPS.isPlaying)
        {
            _myPS.Pause();
        }

        
        ChangeVelocity(_speed);
    }
    private void ChangeVelocity(float speed)
    {
        var _myLiveParts = new ParticleSystem.Particle[_myPS.main.maxParticles];


        int aliveParticlesCount = _myPS.GetParticles(_myLiveParts);

        for(int i = 0; i < aliveParticlesCount; i++)
        {
            _myLiveParts[i].velocity = new Vector3(0, speed,0);
        }
        _myPS.SetParticles(_myLiveParts, aliveParticlesCount);
    }
}
