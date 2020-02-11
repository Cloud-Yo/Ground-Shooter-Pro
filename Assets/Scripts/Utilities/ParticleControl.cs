using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    [SerializeField] private ParticleSystem _myPS;

    // Start is called before the first frame update
    private void Awake()
    {
        _myPS = GetComponent<ParticleSystem>();
        if (_myPS == null)
        {
            Debug.LogError("ParticleSystem is Null: " + gameObject.name);
        }
        
    }

    void Start()
    {

            
    }

    // Update is called once per frame
    void Update()
    {
        var main = _myPS.main;
        main.startSpeed = GameManager._globalSpeed * -4f;

        if (GameManager._globalSpeed == 0)
        {
            _myPS.Stop();
        }

        else if (GameManager._globalSpeed  > 0 && _myPS.isStopped)
        {
            
            _myPS.Play(false);
        }
    }
}
