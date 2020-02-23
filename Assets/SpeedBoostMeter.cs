using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpeedBoostMeter : MonoBehaviour
{
    [SerializeField] private float _length;
    [SerializeField] private float _min = 0.11f, _max = 1.22f;
    [SerializeField] private SpriteRenderer _mySR;
    [SerializeField] private Light2D _fullLight, _emptyLight;
    void Start()
    {
        _length = 1.22f;
        _emptyLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _length = Mathf.Clamp(_length, _min, _max);
        _mySR.size = new Vector2(0.155f, _length);
    }

    public void UpdateLength(float percent)
    {
        _length = percent * _max;
        if(percent == 1f)
        {
            _emptyLight.enabled = false;
            _fullLight.enabled = true;
        }
        else if(percent < 1 && percent > 0.1)
        {
            _fullLight.enabled = false;
        }
        else if(percent < 0.1)
        {
            _emptyLight.enabled = true;
        }
    }


}
