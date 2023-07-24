using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class NavigationGame : MonoBehaviour
{
    public static NavigationGame instance { get; private set; }

    [Header("Объекты меню")]
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _exitMenu;
    [SerializeField] private GameObject _selectCountPlayerPanel;
    [SerializeField] private GameObject _selectGamePanel;

    [Header("Панель игры")]
    [SerializeField] private GameObject _gamePanel;

    [Header("Панель выйграша")]
    [SerializeField] private GameObject _winPanel;

    [Header("Img для затеме")]
    [SerializeField] private Image _faderImage;

    [Header("Скрипты")]
    [SerializeField] private StartGameScript _startGameScript;

    private Tween _fadeScreenTween;

    private int _countPlayerInGame;
    private int _difficultyInGame;


    private bool _inGame;
    private bool _startGameTrigger;
    private bool _exitGameTrigger;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _faderImage.enabled = true;
        _faderImage.color = new Color(0, 0, 0, 1);

        _winPanel.SetActive(false);
        _exitMenu.SetActive(false);
        _gamePanel.SetActive(false);
        _settingsMenu.SetActive(false);
        _selectGamePanel.SetActive(false);
        _selectCountPlayerPanel.SetActive(true);

        FadeScreen(0);
    }

    public void SelectCountPlayer(int countPlayer)
    {
        SoundManagerScript.instance.PressButtonUISound();
        _countPlayerInGame = countPlayer;
        _selectCountPlayerPanel.SetActive(false);
        _selectGamePanel.SetActive(true);
    }

    public void StartGame(int difficulty)
    {
        SoundManagerScript.instance.PressButtonUISound();

        _difficultyInGame = difficulty;

        _startGameTrigger = true;

        FadeScreen(1);

    }

    public void OpenExitMenu()
    {
        SoundManagerScript.instance.PressButtonUISound();
        _exitMenu.SetActive(true);
    }

    public void ExitGame()
    {
        SoundManagerScript.instance.PressButtonUISound();
        PlayerPrefs.SetInt("Restart", 0);

        if (_inGame)
        {
            _inGame = false;
            ExitGameToMenu();
        }
        else
        {
            DOTween.KillAll();
            Application.Quit();
        }

    }

    public void OpenSettingsMenu()
    {
        SoundManagerScript.instance.PressButtonUISound();
        _settingsMenu.SetActive(true);
    }

    private void FadeScreen(float endValue)
    {
        float timeFade = 0.8f;

        if (endValue == 1)
        {
            _faderImage.enabled = true;
            _fadeScreenTween = _faderImage.DOFade(endValue, timeFade).OnComplete(() => FadeScreenOff(endValue));
        }
        else if (endValue == 0)
        {
            if (PlayerPrefs.GetInt("Restart", 0) == 1)
            {
                PlayerPrefs.SetInt("Restart", 0);
                _countPlayerInGame = PlayerPrefs.GetInt("CountPlayer");
                _difficultyInGame = PlayerPrefs.GetInt("Difficulty");
                _inGame = true;
                _startGameTrigger = false;
                OpenGamePanel();
            }

            _fadeScreenTween = _faderImage.DOFade(endValue, timeFade).OnComplete(() => FadeScreenOff(endValue));
        }
    }

    private void FadeScreenOff(float endValue)
    {
        if (endValue == 1)
        {
            _fadeScreenTween.Kill();

            if (_startGameTrigger)
            {
                _inGame = _startGameTrigger;
                _startGameTrigger = false;
                OpenGamePanel();
            }

            if (_exitGameTrigger)
            {
                _exitGameTrigger = false;
                DOTween.KillAll();
                SceneManager.LoadScene(0);
            }
            else
            {
                FadeScreen(0);
            }
        }
        else if (endValue == 0)
        {
            _faderImage.enabled = false;
        }
    }

    private void OpenGamePanel()
    {
        MoveBGForStartMenu.instance.DellBG();

        _settingsMenu.SetActive(false);
        _selectGamePanel.SetActive(false);
        _selectCountPlayerPanel.SetActive(false);
        _gamePanel.SetActive(true);

        _startGameScript.StartGame(_countPlayerInGame, _difficultyInGame);
    }

    private void ExitGameToMenu()
    {
        _exitGameTrigger = true;
        FadeScreen(1);
    }


    public int GetDifficultLevel()
    {
        return _difficultyInGame;
    }

    public int GetCountPlayers()
    {
        return _countPlayerInGame;
    }

    public void OpenWinPanel(int scoreOnePlayer, int scoreTwoPlayer)
    {
        _winPanel.SetActive(true);
        WinnerScript.instance.PlayOpenAnimation(scoreOnePlayer, scoreTwoPlayer);
    }

    public void RestartGame()
    {
        SoundManagerScript.instance.PressButtonUISound();
        _inGame = false;
        PlayerPrefs.SetInt("Restart", 1);
        PlayerPrefs.SetInt("CountPlayer", _countPlayerInGame);
        PlayerPrefs.SetInt("Difficulty", _difficultyInGame);

        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }
}
