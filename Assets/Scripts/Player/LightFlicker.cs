using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Light2D _light;
    [SerializeField] private float _rand;
    void Start()
    {
        _light = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = Random.Range(2f, 3f);
    }
}
