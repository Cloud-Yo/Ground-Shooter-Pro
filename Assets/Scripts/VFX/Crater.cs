using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crater : MonoBehaviour
{
    [SerializeField] private GameObject _crater;


    public void MakeCrater()
    {
        Instantiate(_crater, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 0.5f);
    }
}
