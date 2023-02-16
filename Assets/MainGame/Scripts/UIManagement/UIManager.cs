using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _gameOverText;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _playerMainPanel;
    [SerializeField] private float _countdownTime;
    [SerializeField] private Slider _elixirSlider;
    [SerializeField] private TMP_Text _elixirCountText;

    private float _elixirCount;

    public float ElixirCount
    {
        get => _elixirCount;
    }

    public float CountdownTime
    {
        get => _countdownTime;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _countdownTime = Convert.ToInt32(_countdownText.text);
        StartCoroutine(Countdown());
    }
    private void Update()
    {
        IncreaseElixir();
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

            if (_countdownTime % 15 == 0)
            {
                SpawnManager._Instance.IncereaseSpawnRate();
            }
            // If the countdown time is less than or equal to 0, call the TimeIsUp method
            if (_countdownTime <= 0)
            {
                TimeIsUp();
            }
        }
    }
    private void IncreaseElixir()
    {
        if (_elixirCount <= 1)
        {
            _elixirCount += Time.deltaTime / 10;
            _elixirSlider.value = _elixirCount;
            _elixirCountText.text = ((int)(_elixirCount * 10)).ToString();
        }
    }
    public void DecreaseElixir(float value)
    {
        _elixirCount -= value / 10;
        _elixirSlider.value = _elixirCount;
    }
    private void TimeIsUp()
    {
        if (_countdownTime <= 0)
        {
            _gameOverText.text = "Time is up!";
            _gameOverPanel.SetActive(true);
            _playerMainPanel.SetActive(false);
            Time.timeScale = 0f;
        }
    }
    public void YouWin()
    {
        _gameOverText.text = "You Win!";
        _gameOverPanel.SetActive(true);
        _playerMainPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    public void YouLose()
    {
        _gameOverText.text = "You Lose!";
        _gameOverPanel.SetActive(true);
        _playerMainPanel.SetActive(false);
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
