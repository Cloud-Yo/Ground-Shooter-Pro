using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedShadow : MonoBehaviour
{

    [SerializeField] private GameObject _shadowCaster;
    [SerializeField] private float _shadowOffsetY = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(_shadowCaster.transform.position.x, _shadowCaster.transform.position.y -_shadowOffsetY);
        transform.rotation = _shadowCaster.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _shadowCaster.transform.rotation;
    }
}
