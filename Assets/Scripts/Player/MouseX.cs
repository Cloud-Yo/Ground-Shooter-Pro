using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseX : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(0, 8.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosX = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        transform.position = new Vector2(mousePosX.x, 8.5f);
    }
}
