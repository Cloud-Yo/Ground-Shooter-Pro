using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _leftPropEmpty, _rightPropEmpty;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 myLocalScale;
    [SerializeField] private Animator _myAnimator;



    // Start is called before the first frame update
    void Start()
    {
        _leftPropEmpty = GameObject.Find("bomberPropellerLeft").GetComponent<SpriteRenderer>();
        _rightPropEmpty = GameObject.Find("bomberPropellerRight").GetComponent<SpriteRenderer>();
        _myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInpunt = Input.GetAxis("Vertical");
        _myAnimator.SetFloat("bomberTurn", horizInput);
       
        transform.Translate(new Vector2(horizInput, vertInpunt) * _speed * Time.deltaTime);
        

        if (horizInput > 0.5)
        {
            transform.localScale = new Vector2(-1, 1);
            _rightPropEmpty.flipX = true;
            _leftPropEmpty.flipX = true;

        }
        else if(horizInput < -0.5)
        {
            transform.localScale = new Vector2(1, 1);
            _rightPropEmpty.flipX = false;
            _leftPropEmpty.flipX = false;
        }

    }
}
