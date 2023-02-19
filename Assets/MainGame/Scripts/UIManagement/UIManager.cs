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
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private float _countdownTime;
    [SerializeField] private Slider _elixirSlider;
    [SerializeField] private TMP_Text _elixirCountText;

    #region Download Content

    [SerializeField] private GameObject _downloadMenu, _downloadingMenu, _downloadedMenu, _loadingMenu, _checkingForUpdatesMenu;
    [SerializeField] private GameObject _downloadButton;
    [SerializeField] private TMP_Text _downloadAmountText, _downloadedAmountText, _threeDotsText;
    [SerializeField] private Slider _downloadingSlider;
    [SerializeField] private AddressableSystemController _addressableSystemController;

    #endregion

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
        // When the game starts, set the _countdownTime to the value of the _countdownText.text.
        _countdownTime = Convert.ToInt32(_countdownText.text);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // If the game state is not equal to Game, return
        if (GameManager.Instance._gameState != GameManager.GameState.Game) return;

        // IncreaseElixir();
        IncreaseElixir();
    }

    /// <summary>
    /// If player clicks on the retry button, this method is called. It reloads the current scene.
    /// </summary>
    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// This method is Countdown the time. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        // This loop will run until the countdown time is less than or equal to 0
        while (_countdownTime > 0)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease the countdown time by 1
            _countdownTime--;

            // Update the countdown text
            _countdownText.text = _countdownTime.ToString();

            // If the countdown time is divisible by 15, call the IncereaseSpawnRate method from the SpawnManager.cs
            if (_countdownTime % 15 == 0)
            {
                // Call the IncereaseSpawnRate method from the SpawnManager.cs
                SpawnManager._Instance.IncereaseSpawnRate();
            }

            // If the countdown time is less than or equal to 0, call the TimeIsUp method
            if (_countdownTime <= 0)
            {
                TimeIsUp();
            }
        }
    }

    /// <summary>
    /// This method is Increases the elixir.
    /// </summary>
    private void IncreaseElixir()
    {
        // If the elixir count is less than or equal to 1
        if (_elixirCount <= 1)
        {
            // Increase the elixir count by the time delta time divided by 10.
            _elixirCount += Time.deltaTime / 10;

            // Update the elixir slider value.
            _elixirSlider.value = _elixirCount;

            // Update the elixir count text.
            _elixirCountText.text = ((int)(_elixirCount * 10)).ToString();
        }
    }

    /// <summary>
    /// This method is called from the DragAndDropSystem.cs when the player drops the object on the correct position.
    /// </summary>
    /// <param name="value"> The value of the object that the player dropped. </param>
    public void DecreaseElixir(float value)
    {
        // Decrease the elixir count by the value of the object that the player dropped.
        _elixirCount -= value / 10;

        // Update the elixir slider value.
        _elixirSlider.value = _elixirCount;
    }

    /// <summary>
    /// If the countdown time is less than or equal to 0, the game will be over and the game over panel will be displayed.
    /// </summary>
    private void TimeIsUp()
    {
        // If the countdown time is less than or equal to 0
        if (_countdownTime <= 0)
        {
            // Set the game state to GameOver. This will prevent the game from starting.
            GameManager.Instance._gameState = GameManager.GameState.GameOver;

            // Set the game over text to "Time is up!"
            _gameOverText.text = "Time is up!";

            // Show the game over panel and hide the player main panel
            _gameOverPanel.SetActive(true);
            _playerMainPanel.SetActive(false);
            _countdownPanel.SetActive(false);

            // If the countdown time is less than or equal to 0, set the time scale to 0. This will stop the game.
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// If the player wins, the game will be over and the game over panel will be displayed.
    /// </summary>
    public void YouWin()
    {
        // Set the game state to GameOver. This will prevent the game from starting.
        GameManager.Instance._gameState = GameManager.GameState.GameOver;

        // Set the game over text to "You Win!"
        _gameOverText.text = "You Win!";

        // Show the game over panel and hide the player main panel
        _gameOverPanel.SetActive(true);
        _playerMainPanel.SetActive(false);
        _countdownPanel.SetActive(false);

        // If the player wins, set the time scale to 0. This will stop the game.
        Time.timeScale = 0f;
    }

    /// <summary>
    /// If the player loses, the game will be over and the game over panel will be displayed.
    /// </summary>
    public void YouLose()
    {
        // Set the game state to GameOver. This will prevent the game from starting.
        GameManager.Instance._gameState = GameManager.GameState.GameOver;

        // Set the game over text to "You Lose!"
        _gameOverText.text = "You Lose!";

        // Show the game over panel and hide the player main panel
        _gameOverPanel.SetActive(true);
        _playerMainPanel.SetActive(false);
        _countdownPanel.SetActive(false);

        // If the player loses, set the time scale to 0. This will stop the game.
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

    #region Download Content Methods

    /// <summary>
    /// If the player clicks the download button, the game will start to download the content. Only in download menu.
    /// </summary>
    public void DownloadButton()
    {
        // Set the game state to Updating. This will prevent the game from starting.
        GameManager.Instance._gameState = GameManager.GameState.Updating;

        // Show the downloading menu
        ShowDownloadingMenu();

        // Start the download process. This will download the content and update the UI.
        _addressableSystemController.StartInitializeAddressables();
    }

    /// <summary>
    /// If the player clicks the play button, the game will start. Only in download menu.
    /// </summary>
    public void PlayButton()
    {
        // Set the game time scale to 1. This will start the game.
        Time.timeScale = 1f;

        // Set the game state to UpdateSuccess. This will allow the game to start.
        GameManager.Instance._gameState = GameManager.GameState.Game;

        // Close the loading menu
        _loadingMenu.SetActive(false);

        // Show the player main panel and countdown panel
        _playerMainPanel.SetActive(true);
        _countdownPanel.SetActive(true);

        // Start the SpawnCoroutine
        SpawnManager._Instance.StartSpawnCoroutine();

        // Start the countdown coroutine
        StartCoroutine(Countdown());
    }

    /// <summary>
    /// Close check for updates menu and active download menu. Only in download menu.
    /// </summary>
    public void ShowDownloadMenu()
    {
        _checkingForUpdatesMenu.SetActive(false);
        _downloadMenu.SetActive(true);
    }

    /// <summary>
    /// If proccess is downloading, show the downloading menu. Only in download menu. Downloading menu is the menu that shows the download progress. Slider and text.
    /// </summary>
    public void ShowDownloadingMenu()
    {
        _downloadMenu.SetActive(false);
        _downloadingMenu.SetActive(true);

    }

    /// <summary>
    /// If proccess is downloaded, show the downloaded menu. Only in download menu.
    /// </summary>
    public void ShowDownloadedMenu()
    {
        _downloadingMenu.SetActive(false);
        _downloadedMenu.SetActive(true);
    }

    /// <summary>
    /// If update check proccess is finished, show the download button. Only in download menu. Download button is the button that starts the download process.
    /// </summary>
    public void ShowDownloadButton()
    {
        _downloadButton.SetActive(true);
    }

    /// <summary>
    /// This method will set the download amount text. Only in download menu. Amount text is the text that shows the download amount. For example: 0.5 MB
    /// </summary>
    /// <param name="text"> The text that will be set to the download amount text. </param>
    public void SetDownloadAmountText(string text)
    {
        // Set the text to the download amount text
        _downloadAmountText.text = text;
    }

    /// <summary>
    /// This method will set the three dots text. Only in download menu. Three dots text is the text that shows the three dots animation. For example: -> . -> .. -> ...
    /// </summary>
    /// <param name="text"></param>
    public void SetThreeDotsText(string text)
    {
        _threeDotsText.text = text;
    }

    /// <summary>
    /// This method will set the download amount to the slider and text. Only in download menu. Slider and text are the slider and text that shows the download progress. For example: 50%
    /// </summary>
    /// <param name="value"> The value that will be set to the slider and text. </param>
    public void SetDownloadingSliderValueAndTextValue(float value)
    {
        // Set the value to the slider and text
        _downloadingSlider.value += value;
        _downloadedAmountText.text = ((int)(_downloadingSlider.value * 100)).ToString() + "%";

        // If the slider value is 1, call the ShowDownloadedMenu method
        if (_downloadingSlider.value == 1)
        {
            // Show the downloaded menu.
            // This will show the play button.
            ShowDownloadedMenu();
        }
    }

    #endregion
}
