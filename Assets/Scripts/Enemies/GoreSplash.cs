using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreSplash : MonoBehaviour
{
    [SerializeField] private ParticleSystem _AlienGore;
    [SerializeField] private AudioSource _parentAS;
    [SerializeField] private AudioClip _goreSplashClip;


    void Start()
    {
        _AlienGore = GetComponentInParent<ParticleSystem>();
        _AlienGore.Stop();
        var main = _AlienGore.main;
        main.loop = true;
        main.startDelay = 0;
        var emit = _AlienGore.emission;
        emit.rateOverTime = 100;
        var vel = _AlienGore.velocityOverLifetime;
        vel.x = 0.5f;
        vel.y = -2.5f;
        vel.z = 0;
        var trails = _AlienGore.trails;
        trails.enabled = false;
        _parentAS = GetComponentInParent<AudioSource>();
        
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            _AlienGore.Play();
            if(!_parentAS.isPlaying)
            {
                _parentAS.PlayOneShot(_goreSplashClip, .5f);
            }
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

            _AlienGore.Stop();
        
    }
}
