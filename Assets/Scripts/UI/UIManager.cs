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
