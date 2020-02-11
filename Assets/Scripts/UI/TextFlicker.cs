using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFlicker : MonoBehaviour
{
    [SerializeField] private Text _myText;
    [SerializeField] private float _flickerTime;
    // Start is called before the first frame update
    void Start()
    {
        _myText = GetComponent<Text>();
        StartCoroutine(TextFlickerRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TextFlickerRoutine()
    {
       while(true)
        {
            yield return new WaitForSeconds(_flickerTime);
            _myText.enabled = false;
            yield return new WaitForSeconds(_flickerTime);
            _myText.enabled = true;
        }
   
    }
}
