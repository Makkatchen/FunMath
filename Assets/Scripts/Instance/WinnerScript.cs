using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScript : MonoBehaviour
{
    public static WinnerScript instance { get; private set; }

    [Header("Панели для отображения выигрыша")]
    [SerializeField] private GameObject _twoPlayersPanel;
    [SerializeField] private GameObject _onePlayersPanel;

    [Header("Тексты для двух игроков")]
    [SerializeField] private Text _namePlayerText;
    [SerializeField] private Text _labelWinText;
    [SerializeField] private Text _scoreForOnePlayerText;
    [SerializeField] private Text _scoreForTwoPlayerText;

    [Header("Тексты для одного игроков")]
    [SerializeField] private Text _levelValueLabel;
    [SerializeField] private Text _scoreValueLabel;
    [SerializeField] private Text _countScoreLabel;

    [Header("Слайдер для отображения прогресса")]
    [SerializeField] private Slider _progressSlider;

    [Header("Объекты награды")]
    [SerializeField] private GameObject _medalImage;
    [SerializeField] private Sprite _goldMedalSprite;
    [SerializeField] private Sprite _silverMedalSprite;
    [SerializeField] private Sprite _bronzeMedalSprite;

    [Header("Эффекты для начисления очков и левела")]
    [SerializeField] private GameObject _addLevelEffect;
    [SerializeField] private GameObject _shineMedalEffect;

    [Header("Аниматор")]
    [SerializeField] private Animator _animator;

    [Header("Кол-во очков до уровня")]
    [SerializeField] private int _needScoreForLevel;

    /*Переменные для скрипта*/

    private int _levelUser;
    private int _scoreUser;

    private int _scoreReceived;

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

    public void PlayOpenAnimation(int scoreOnePlayer, int scoreTwoPlayer)
    {
        _shineMedalEffect.SetActive(false);
        _addLevelEffect.SetActive(false);
        _medalImage.SetActive(false);

        if (NavigationGame.instance.GetCountPlayers() == 1)
        {
            _twoPlayersPanel.SetActive(false);
            _onePlayersPanel.SetActive(true);
            _levelUser = PlayerPrefs.GetInt("Level", 1);
            _scoreUser = PlayerPrefs.GetInt("Score", 0);
            _scoreValueLabel.text = _scoreUser.ToString();
            _countScoreLabel.text = _needScoreForLevel.ToString();
            _levelValueLabel.text = _levelUser.ToString();
            _progressSlider.value = (float)_scoreUser / _needScoreForLevel;
            _scoreReceived = scoreOnePlayer;
        }
        else
        {
            _twoPlayersPanel.SetActive(true);
            _onePlayersPanel.SetActive(false);
            _scoreForOnePlayerText.text = scoreOnePlayer.ToString();
            _scoreForTwoPlayerText.text = scoreTwoPlayer.ToString();

            string lang = PlayerPrefs.GetString("Lang", "En");

            string firstPlayer = (lang == "En")? "first player!" : "первый игрок!";
            string secondPlayer = (lang == "En") ? "second player!" : "второй игрок!";
            string allPlayer = (lang == "En") ? "Everyone!!!" : "Все!!!";
            string winLabel = (lang == "En") ? "Won:" : (scoreOnePlayer == scoreTwoPlayer) ? "Выйграли: " : "Выйграл: ";

            _namePlayerText.text = (scoreOnePlayer > scoreTwoPlayer) ? firstPlayer : (scoreOnePlayer < scoreTwoPlayer) ? secondPlayer : allPlayer;
            _labelWinText.text = winLabel;
        }


        _animator.Play("OpenWinPanel");

        if (NavigationGame.instance.GetCountPlayers() == 1)
            StartWinScript(_scoreReceived);
    }

    private void StartWinScript(int scoreOnePlayer)
    {

        _medalImage.GetComponent<Image>().sprite = (scoreOnePlayer == 5) ? _goldMedalSprite : (scoreOnePlayer > 3) ? _silverMedalSprite : _bronzeMedalSprite;
        int mode = (NavigationGame.instance.GetDifficultLevel() == 0) ? 5 : (NavigationGame.instance.GetDifficultLevel() == 1)? 10 : 15;
        int addScore = scoreOnePlayer * mode;

        if ((addScore + _scoreUser >= _needScoreForLevel))
        {
            PlayerPrefs.SetInt("Level", ++_levelUser);
            int score = (addScore + _scoreUser) - _needScoreForLevel;
            PlayerPrefs.SetInt("Score", score);
        }
        else
        {
            PlayerPrefs.SetInt("Score", addScore + _scoreUser);
        }

        StartCoroutine(AddScore(addScore, _needScoreForLevel, (addScore + _scoreUser >= _needScoreForLevel)));

    }

    private IEnumerator AddScore(int score, int countScoreForAddLevel, bool isNewLevel)
    {
        yield return new WaitForSeconds(1.25f);
        int stepForAddScore = 1;
        float timeDurationForAddScore = 0.05f;
        int showScore = _scoreUser;
        int remainder = (isNewLevel) ? (score + _scoreUser) - countScoreForAddLevel : 0;
       
        score -= remainder;

        while (score > 0)
        {
            if (score - stepForAddScore >= 0)
            {
                score -= stepForAddScore;
                showScore += stepForAddScore;
            }
            else
            {
                showScore += score;
                score = 0;
            }

            _progressSlider.value = (float)showScore / countScoreForAddLevel;

            _scoreValueLabel.text = showScore.ToString();

            yield return new WaitForSeconds(timeDurationForAddScore);
        }

        if (isNewLevel)
        {
            StartCoroutine(AddLevel(remainder));
        }
        else
        {
            SoundManagerScript.instance.PlaySoundEffects("addMedal");
            _medalImage.SetActive(true);
            _animator.Play("AddMedal");
            yield return new WaitForSeconds(1f);
            ShineMedal();
        }

    }

    private IEnumerator AddLevel(int addScore)
    {
        yield return new WaitForSeconds(0.5f);
        SoundManagerScript.instance.PlaySoundEffects("updateLevelValue");
        _levelUser++;
        _scoreUser = 0;
        _progressSlider.value = (float)_scoreUser / _needScoreForLevel;
        _levelValueLabel.text = _levelUser.ToString();
        _addLevelEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(AddScore(addScore, _needScoreForLevel, false));
    }

    public void ShineMedal()
    {
        SoundManagerScript.instance.PlaySoundEffects("shineMedal");
        _shineMedalEffect.SetActive(true);
    }
}
