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

    [Header("Поле ввода ответа")]
    [SerializeField] private InputField _answerInput;

    [Header("Объекты прогресса игроков")]
    [SerializeField] private GameObject _playerOneProgressPanel;
    [SerializeField] private GameObject _playerTwoProgressPanel;

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

    /*Анимации и твины*/

    private Tween _explusionStarTween;
    private Tween _loseStarTween;
    private Tween _showNumberPlayer;

    /*Переменные для скрипта*/
    private int[] _numbersLowDifficulty = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int[] _numbersNormalDifficulty = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private char[] _operators = new char[] { '+', '-' };

    private int _countExemple;
    private int _countRounds;

    private int _numberActivPlayer;

    private bool _gameIsFinish;

    private int _scoreOnePlayer;
    private int _scoreTwoPlayer;


    public void StartGame(int countPlayer, int difficulty)
    {
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
            _answerInput.text = "???";
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

           if(_countExemple <= _countRounds) ShowBoardWithNumberPlayer();
        }
        else
        {
            _numberActivPlayer = 1;
        }
    }

    private void CreateBoardForCountPlayers(int countPlayers)
    {
        _playerOneProgressPanel.SetActive(true);

        if (countPlayers == 1)
        {
            _namePlayerOne.text = "Ваш успех!";
            _playerTwoProgressPanel.SetActive(false);

        }
        else if (countPlayers == 2)
        {
            _namePlayerOne.text = "Первый игрок!";
            _namePlayerTwo.text = "Второй игрок!";
            _playerTwoProgressPanel.SetActive(true);
        }
    }

    private void ShowBoardWithNumberPlayer()
    {
        float startPositionY = _selectPlayerPanel.transform.position.y;
        _selectPlayerLabel.text = (_numberActivPlayer == 1) ? "Ход игрока №1" : "Ход игрока №2";
        _showNumberPlayer = _selectPlayerPanel.transform.DOMoveY(0, 1.2f).SetDelay(1f).OnComplete(() => ReturnBoardToPlace(startPositionY));
    }

    private void ReturnBoardToPlace(float y)
    {
        _showNumberPlayer.Kill();

        _showNumberPlayer = _selectPlayerPanel.transform.DOMoveY(y, 0.5f).SetDelay(0.8f).OnComplete(() => _showNumberPlayer.Kill());

    }



    public void SubmitAnswerEndEditInputField()
    {
        if (!_gameIsFinish)
        {
            if (InsertAnswerIsCorrecly())
            {
                int answer = int.Parse(_answerInput.text);
                int checkAnswer = GetSolution();

                CheckAnswer(checkAnswer, answer);

                CreateExemple(NavigationGame.instance.GetDifficultLevel());
            }
            else
            {
                SoundManagerScript.instance.PressButtonUISound();
                Debug.Log("Answer is not correcly");
            }
        }
    }

    public void SubmitPressButton()
    {

    }

    private bool InsertAnswerIsCorrecly()
    {
        return (_answerInput.text != "" && _answerInput.text != "???");
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

        int numberStar = ((NavigationGame.instance.GetCountPlayers() == 2) && _numberActivPlayer == 2) ? _countExemple / 2 -1 :
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
        //SoundManagerScript.instance.PlaySoundEffects("explosionStar");
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
        yield return new WaitForSeconds(1.5f);

        SoundManagerScript.instance.PlaySoundEffects("winGame");
        NavigationGame.instance.OpenWinPanel(_scoreOnePlayer, _scoreTwoPlayer);
    }



}
