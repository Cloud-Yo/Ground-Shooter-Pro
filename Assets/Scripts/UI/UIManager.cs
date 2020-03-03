using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private int _enKilled;
    [SerializeField] private Text _enKillTxt;
    [SerializeField] private int _enMissed;
    [SerializeField] private Text _enMissTxt;
    [SerializeField] private int _humSaved;
    [SerializeField] private Text _humSavedTxt;
    [SerializeField] private int _humKilled;
    [SerializeField] private Text _humKillTxt;
    [SerializeField] private GameObject _gameOverTxt;
    [SerializeField] private int _hiScore = 0;
    [SerializeField] private Text _hiScoreTXT;

 



    private void Start()
    {
        _hiScore = PlayerPrefs.GetInt("hiScore");
        if (_hiScore == 0)
        {
            _hiScoreTXT.text = "0000";
        }
        else
        {
            if(_hiScore < 1000 && _hiScore > 99)
            {
                _hiScoreTXT.text = "0" + _hiScore.ToString();
            }
            else if(_hiScore <= 99 && _hiScore > 9)
            {
                _hiScoreTXT.text = "00" +_hiScore.ToString();
            }
         
        }
    }



    public void EnemyKillScore()
    {
        _enKilled += 1;
        _enKillTxt.text = _enKilled.ToString();
    }

    public void EnemyMissedScore()
    {
        _enMissed += 1;
        _enMissTxt.text = _enMissed.ToString();
    }

    /*
    public void HumanSavedScore()
    {
        _humSaved += 1;
        _humSavedTxt.text = _humSaved.ToString();
    }

    public void HumanKilledScore()
    {
        _humKilled += 1;
        _humKillTxt.text = _humKilled.ToString();
    }
    */

    public void GameOverMessage()
    {
        _gameOverTxt.SetActive(true);
    }



}
