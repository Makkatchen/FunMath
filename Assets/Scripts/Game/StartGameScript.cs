using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class StartGameScript : MonoBehaviour
{
    [Header("Текстовые поля для примера")]
    [SerializeField] private Text _oneNumberText;
    [SerializeField] private Text _twoNumberText;
    [SerializeField] private Text _operatorText;

    [Header("Текст отображения ответа")]
    [SerializeField] private Text _answerText;

    [Header("Объекты прогресса игроков")]
    [SerializeField] private GameObject _playerOneProgressPanel;
    [SerializeField] private GameObject _playerTwoProgressPanel;
    [SerializeField] private GameObject _faderForPlayerOneProgressPanel;
    [SerializeField] private GameObject _faderForplayerTwoProgressPanel;

    [Header("Спрайты звезд для прогресса")]
    [SerializeField] private Sprite _winStar;
    [SerializeField] private GameObject _winStarEffectPrefab;
    [SerializeField] private GameObject _loseStarPrefab;
    [SerializeField] private Sprite _emptyStar;

    [Header("Цвета для текста")]
    [SerializeField] private Color[] _colorsForNumbers;

    [Header("Названия игроков")]
    [SerializeField] private Text _namePlayerOne;
    [SerializeField] private Text _namePlayerTwo;

    [Header("Выбор игрока при совместной игре")]
    [SerializeField] private GameObject _selectPlayerPanel;
    [SerializeField] private Text _selectPlayerLabel;

    [Header("Vertical Layout Group для прогресса игроков")]
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroupForProgress;

    [Header("Lock Image for game")]
    [SerializeField] GameObject _lockImage;
    [SerializeField] GameObject _lockImageForRewardButton;

    [Header("Reward button")]
    [SerializeField] Button _rewardButton;


    /*Анимации и твины*/

    private Tween _explusionStarTween;
    private Tween _loseStarTween;
    private Tween _showNumberPlayer;

    /*Переменные для скрипта*/
    private char[] _operators = new char[] { '+', '-' };

    private int _countExemple;
    private int _countRounds;

    private int _numberActivPlayer;

    private bool _gameIsFinish;

    private int _scoreOnePlayer;
    private int _scoreTwoPlayer;


    public void StartGame(int countPlayer, int difficulty)
    {
        _rewardButton.interactable = false;
        _faderForPlayerOneProgressPanel.SetActive(false);
        _faderForplayerTwoProgressPanel.SetActive(false);

        _gameIsFinish = false;

        SelectBGForGame.instance.SelectBg();

        _countRounds = (countPlayer == 1) ? 5 : 10;

        CreateBoardForCountPlayers(countPlayer);

        CreateExemple(difficulty);
    }

    private void CreateExemple(int difficulty)
    {
        CountExempleAndSelectPlayer();

        if (_countExemple > _countRounds)
        {
            _gameIsFinish = true;

            StartCoroutine(FinishGame());
        }
        else
        {
            _answerText.text = "???";
            _operatorText.text = _operators[Random.Range(0, _operators.Length)].ToString();
            int oneNumber = 0;
            int twoNumber = 0;

            if (difficulty == 0)
            {
                oneNumber = Random.Range(1, 10);
                twoNumber = Random.Range(1, 10);
            }
            else if (difficulty == 1)
            {
                oneNumber = Random.Range(10, 20);
                twoNumber = Random.Range(1, 10);
            }
            else if (difficulty == 2)
            {
                oneNumber = Random.Range(10, 20);
                twoNumber = Random.Range(5, 20);
            }


            if (_operatorText.text == "-")
            {
                if (oneNumber < twoNumber)
                {
                    int tmp = oneNumber;
                    oneNumber = twoNumber;
                    twoNumber = tmp;
                }
            }

            SelectColorForNumbers();

            _oneNumberText.text = oneNumber.ToString();
            _twoNumberText.text = twoNumber.ToString();

            TimerForRewardButtonScript.instance.StartTimer();
        }

    }

    private void SelectColorForNumbers()
    {
        int randomColorOne = Random.Range(0, _colorsForNumbers.Length);
        int randomColorTwo = Random.Range(0, _colorsForNumbers.Length);
        if (randomColorOne == randomColorTwo)
        {
            while (randomColorOne == randomColorTwo)
            {
                randomColorTwo = Random.Range(0, _colorsForNumbers.Length);
            }
        }

        _oneNumberText.color = _colorsForNumbers[randomColorOne];
        _twoNumberText.color = _colorsForNumbers[randomColorTwo];
    }

    private void CountExempleAndSelectPlayer()
    {
        _countExemple++;

        if (NavigationGame.instance.GetCountPlayers() == 2)
        {
            if (_countExemple % 2 != 0) _numberActivPlayer = 1;
            else _numberActivPlayer = 2;

            if (_countExemple <= _countRounds) ShowBoardWithNumberPlayer();
        }
        else
        {
            _numberActivPlayer = 1;
        }
    }

    private void CreateBoardForCountPlayers(int countPlayers)
    {
        string lang = PlayerPrefs.GetString("Lang", "En");

        _playerOneProgressPanel.SetActive(true);
        if (countPlayers == 1)
        {
            _namePlayerOne.text = (lang == "En") ? "Your progress!" : "Ваш успех!";
            _playerTwoProgressPanel.transform.parent.gameObject.SetActive(false);

            _verticalLayoutGroupForProgress.childControlHeight = false;
            _playerOneProgressPanel.transform.parent.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
                

        }
        else if (countPlayers == 2)
        {
            _namePlayerOne.text = (lang == "En") ? "First Player!" : "Первый игрок!";
            _namePlayerTwo.text = (lang == "En") ? "Second Player!" : "Второй игрок!";
            _playerTwoProgressPanel.SetActive(true);
        }
    }

    private void ShowBoardWithNumberPlayer()
    {
        string lang = PlayerPrefs.GetString("Lang", "En");
        string nameActivPlayer = (lang == "En") ? "Player Turn " : "Ход игрока ";
        _lockImage.SetActive(true);
        float startPositionY = _selectPlayerPanel.transform.position.y;
        _selectPlayerLabel.text = (_numberActivPlayer == 1) ? nameActivPlayer + "#1" : nameActivPlayer + "#2";
        _showNumberPlayer = _selectPlayerPanel.transform.DOMoveY(0, 0.8f).SetDelay(0.8f).OnComplete(() => ReturnBoardToPlace(startPositionY));
    }

    private void ReturnBoardToPlace(float y)
    {
        bool setFadeForPanel = (_numberActivPlayer == 1) ? true : false;
        _faderForPlayerOneProgressPanel.SetActive(!setFadeForPanel);
        _faderForplayerTwoProgressPanel.SetActive(setFadeForPanel);
        _showNumberPlayer.Kill();
        _showNumberPlayer = _selectPlayerPanel.transform.DOMoveY(y, 0.3f).SetDelay(0.8f).OnComplete(() => CompleteShowBoard());
    }

    private void CompleteShowBoard()
    {
        _showNumberPlayer.Kill();
        _lockImage.SetActive(false);
    }

    public void SubmitPressButton()
    {
        if (!_gameIsFinish)
        {
            if (InsertAnswerIsCorrecly())
            {
                int answer = int.Parse(_answerText.text);
                int checkAnswer = GetSolution();

                CheckAnswer(checkAnswer, answer);

                CreateExemple(NavigationGame.instance.GetDifficultLevel());

                ShowRewardButton(false);
            }
            else
            {
                SoundManagerScript.instance.PressButtonUISound();
                Debug.Log("Answer is not correcly");
            }
        }
    }

    private bool InsertAnswerIsCorrecly()
    {
        return (_answerText.text != "" && _answerText.text != "???");
    }



    private void CheckAnswer(int checkAnswer, int answer)
    { 
        if (_numberActivPlayer == 1)
        {
            AddResultInBoardPlayer(_playerOneProgressPanel, (answer == checkAnswer));
            _scoreOnePlayer = (answer == checkAnswer) ? _scoreOnePlayer + 1 : _scoreOnePlayer;
        }
        else if (_numberActivPlayer == 2)
        {
            AddResultInBoardPlayer(_playerTwoProgressPanel, (answer == checkAnswer));
            _scoreTwoPlayer = (answer == checkAnswer) ? _scoreTwoPlayer + 1 : _scoreTwoPlayer;
        }
    }

    private void AddResultInBoardPlayer(GameObject playerPanel, bool isCorrectly)
    {
        Image[] stars = playerPanel.transform.Find("HorizontalGroupForStars").gameObject.GetComponentsInChildren<Image>();

        int numberStar = ((NavigationGame.instance.GetCountPlayers() == 2) && _numberActivPlayer == 2) ? _countExemple / 2 - 1 :
            (NavigationGame.instance.GetCountPlayers() == 2)?_countExemple - (_countExemple / 2) - 1 : _countExemple-1;

        if (isCorrectly)
        {
            SoundManagerScript.instance.PlaySoundEffects("correctInsert");
            _explusionStarTween = stars[numberStar].transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).OnComplete(() => CompleteAnimationForExplusionStart(stars[numberStar].transform));
            stars[numberStar].sprite = _winStar;

        }
        else
        {
            stars[numberStar].sprite = _emptyStar;
            GameObject loseObj = Instantiate(_loseStarPrefab, stars[numberStar].transform);
            _loseStarTween = loseObj.transform.DOScale(Vector3.one, 0.8f);
            SoundManagerScript.instance.PlaySoundEffects("errorInsert");
        }
    }

    private void CompleteAnimationForExplusionStart(Transform star)
    {
        Instantiate(_winStarEffectPrefab, star);
        _explusionStarTween.Kill();
        _explusionStarTween = star.DOScale(Vector3.one, 0.5f);
    }


    private int GetSolution()
    {
        int checkAnswer = 0;

        if (_operatorText.text == "+")
            checkAnswer = int.Parse(_oneNumberText.text) + int.Parse(_twoNumberText.text);
        else if (_operatorText.text == "-")
            checkAnswer = int.Parse(_oneNumberText.text) - int.Parse(_twoNumberText.text);

        return checkAnswer;
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManagerScript.instance.PlaySoundEffects("winGame");
        NavigationGame.instance.OpenWinPanel(_scoreOnePlayer, _scoreTwoPlayer);
    }


    public void OnClickNumberButton(int number)
    {
        SoundManagerScript.instance.PressButtonUISound();
        if(_answerText.text.Length < 4)
            _answerText.text = (_answerText.text == "???") ? number.ToString() : _answerText.text + number.ToString();
    }

    public void OnClickClearButton()
    {
        SoundManagerScript.instance.PressButtonUISound();
        _answerText.text = "???";
    }

    public void ShowAnswerForReward()
    {
        _answerText.text = GetSolution().ToString();
        ShowRewardButton(false);
    }

    public void ShowRewardButton(bool isShow)
    {
        if (isShow)
        {
            _rewardButton.interactable = isShow;
            _lockImageForRewardButton.SetActive(isShow);
            Image fade = _lockImageForRewardButton.GetComponent<Image>();
            fade.color = new Color(0, 0.7607f, 0.8313f, 1);
            fade.DOFade(0, 0.8f).OnComplete(() =>
            {
                _lockImageForRewardButton.SetActive(!isShow);
            }
            );
        }
        else
        {
            Image fade = _lockImageForRewardButton.GetComponent<Image>();
            fade.color = new Color(0, 0.7607f, 0.8313f, 1);
            _lockImageForRewardButton.SetActive(!isShow);
            _rewardButton.interactable = isShow;
        }

    }


}
