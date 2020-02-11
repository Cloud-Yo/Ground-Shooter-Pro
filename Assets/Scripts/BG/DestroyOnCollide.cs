using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollide : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private GameObject[] _lights = new GameObject[2];

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for(int i = 0; i < _lights.Length; i++)
            {
                _lights[i].SetActive(false);
            }
            _myAnimator.SetTrigger("Break");
        }
    }

}
