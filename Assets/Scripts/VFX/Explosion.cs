using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] private AudioClip _explosionClip;


    void Start()
    {
        AudioSource.PlayClipAtPoint(_explosionClip, Camera.main.transform.position, 0.5f);
    }

    // Update is called once per frame


    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
