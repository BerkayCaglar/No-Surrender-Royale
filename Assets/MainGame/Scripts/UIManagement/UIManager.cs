using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager _Instance { get; private set; }
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private float _countdownTime;
    public float CountdownTime
    {
        get => _countdownTime;
    }

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
    private void Start()
    {
        _countdownTime = Convert.ToInt32(_countdownText.text);
        StartCoroutine(Countdown());
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Countdown()
    {
        while (_countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _countdownTime--;
            _countdownText.text = _countdownTime.ToString();

            // If the countdown time is less than or equal to 0, call the TimeIsUp method
            if (_countdownTime <= 0)
            {
                TimeIsUp();
            }
        }
    }
    private void TimeIsUp()
    {
        if (_countdownTime <= 0)
        {
            _gameOverText.text = "Time is up!";
            _gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void YouWin()
    {
        _gameOverText.text = "You Win!";
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void YouLose()
    {
        _gameOverText.text = "You Lose!";
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Kullanımda Değil.
    /// </summary>
    public void PauseButton()
    {
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
    }
    /// <summary>
    /// Kullanımda Değil.
    /// </summary>
    public void ResumeButton()
    {
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
    }
}
