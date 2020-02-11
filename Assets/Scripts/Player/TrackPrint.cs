using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPrint : MonoBehaviour
{
    [SerializeField] private ParticleSystem _myPS;
    [SerializeField] private Color _defaultCol;
    [SerializeField] private Color _goreCol;
    [SerializeField] private float _speed = 1f;
    
    void Start()
    {
        _myPS = GetComponent<ParticleSystem>();

    }

    private void Update()
    {
        var main = _myPS.main;
        if (main.startColor.color != _defaultCol)
        {
            main.startColor = Color.Lerp(main.startColor.color, _defaultCol, _speed * Time.deltaTime);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Gore")
        {
            var main = _myPS.main;
            main.startColor = _goreCol;

        }
    }


}
