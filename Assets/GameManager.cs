using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{


    [SerializeField] private bool _isGameOver = false;

    public static float _globalSpeed;
    public static Vector2 _globalShadowOffset;
    [SerializeField, Range(-10f, 10f)] private float _gsoX, _gsoY;
    public static Color _globalShadowColor;
    [SerializeField] private Color _shadowColorEdit;
    [SerializeField] private FadeControls _fadetoBlack;

    
    // Start is called before the first frame update
    void Start()
    {
        _globalSpeed = 0;
        _fadetoBlack = GameObject.Find("BlackFade").GetComponent<FadeControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);

        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        _globalShadowOffset = new Vector2(_gsoX, _gsoY);
        _globalShadowColor = _shadowColorEdit;
    }

    public void GameIsOver()
    {
        _isGameOver = true;
        _fadetoBlack.FadeOut();
    }

    public void SetGlobalSpeed(float speed)
    {
        _globalSpeed = speed;
    }
}
