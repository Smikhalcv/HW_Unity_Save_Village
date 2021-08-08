using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ImageTimer _seedTimer;
    [SerializeField] private Image _peasantTimer;
    [SerializeField] private Image _warriorTimer;
    [SerializeField] private ImageTimer _enemyTimer;
    [SerializeField] private ImageTimer _eatingTimer;

    [SerializeField] private Button _warriorCreateButton;
    [SerializeField] private Button _peasantCreateButton;

    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _strikeSword;
    [SerializeField] private GameObject _sounds;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [SerializeField] private GameObject _firstScene;
    [SerializeField] private GameObject _manualScene;
    private static bool _manualSceneCheck = true;
    [SerializeField] private GameObject _gameScene;
    [SerializeField] private GameObject _winScene;
    [SerializeField] private Text _resultWinValue;
    [SerializeField] private GameObject _failScene;
    [SerializeField] private Text _resultLoseValue;

    [SerializeField] private int _grainWinningCondition;
    [SerializeField] private int _peasantWinningCondition;

    public int countPeasant;
    public int productionSeedPeasant;
    [SerializeField] private int _timeForCraetePeasant;
    [SerializeField] private int _costPeasant;
    public int countWarrior;
    public int eatingWarrior;
    [SerializeField] private int _timeForCraeteWarrior;
    [SerializeField] private int _costWarrior;
    public int countSeed;
    public Text resourText;
    public Text valueCountEnemy;

    [SerializeField] private int _countEnemy;

    private static GameObject _currentScene;
    private static bool _muteFlag = true;
    private static float _timeHireWarrior = -2;
    private static float _timeHirePeasant = -2;

    private static int _totalPeasant = 0;
    private static int _totalWarrior = 0;
    private static int _totalSeed = 0;
    [SerializeField] private int _totalWave = 0;
    private static int _totalFoodEaten = 0;

    private static bool _playGame = true;

    private static System.Random random = new System.Random();

    private void Start()
    {
        UpdateText();
        _backgroundMusic.Play();
        _firstScene.SetActive(true);
        _currentScene = _firstScene;
    }

    private void OnEnable()
    {
        countPeasant = 5;
        countSeed = 0;
        countWarrior = 0;
        _countEnemy = 0;
        _totalFoodEaten = 0;
        _totalPeasant = 0;
        _totalSeed = 0;
        _totalWarrior = 0;
        _totalWave = 0;
        _timeHirePeasant = -2;
        _timeHireWarrior = -2;
    }

    private void Update()
    {
        if (_playGame)
        {
            CheckTick();
            TimerCreateWarrior();
            CheckInteractableButtonForHireWarrior();
            TimerCreatePeasant();
            CheckInteractableButtonForHirePeasant();
            ConditionWin();
            UpdateText();
        }
    }

    /// <summary>
    /// Меняет сцену на указанную и закрывает предыдущую
    /// </summary>
    /// <param name="nextScene"></param>
    public void ChangeScene(GameObject nextScene)
    {
        _currentScene.SetActive(false);
        _currentScene = nextScene;
        _currentScene.SetActive(true);
    }

    /// <summary>
    /// Меняет изображение кнопки выключить звук
    /// </summary>
    /// <param name="muteButton">Кнопка для блокировки звука</param>
    public void ChangeSpriteSoundButton(Button muteButton)
    {
        if (_muteFlag)
        {
            muteButton.image.sprite = _soundOff;
            _sounds.SetActive(false);
            _muteFlag = false;
        }
        else
        {
            muteButton.image.sprite = _soundOn;
            _sounds.SetActive(true);
            _backgroundMusic.Play();
            _muteFlag = true;
        }
    }

    /// <summary>
    /// запускает звук клика на кнопку
    /// </summary>
    public void ClickSound()
    {
        _clickSound.Play();
    }

    /// <summary>
    /// Запускает отсчёт создания война и блокирует кнопку создания войнов, отнимает стоимость война в зернах
    /// </summary>
    public void CreateWarrior()
    {
        countPeasant -= 1;
        countSeed -= _costWarrior;
        _timeHireWarrior = _timeForCraeteWarrior;
        _warriorCreateButton.interactable = false;
    }

    /// <summary>
    /// Отсчитывает время создания война
    /// </summary>
    private void TimerCreateWarrior()
    {
        if (_timeHireWarrior > 0)
        {
            _timeHireWarrior -= Time.deltaTime;
            _warriorTimer.fillAmount = 1 - _timeHireWarrior / _timeForCraeteWarrior;
        }
        else if (_timeHireWarrior > -1)
        {
            countWarrior += 1;
            _totalWarrior++;
            _timeHireWarrior = -2;
            _warriorTimer.fillAmount = 0;
        }
    }

    /// <summary>
    /// Проверяет нужно ли блокировать кнопку найма воинов
    /// </summary>
    private void CheckInteractableButtonForHireWarrior()
    {
        if (_costWarrior > countSeed || _timeHireWarrior > 0 || countPeasant <= 0)
        {
            _warriorCreateButton.interactable = false;
        }
        else
        {
            _warriorCreateButton.interactable = true;
        }
    }

    /// <summary>
    /// Проверяет нужно ли блокировать кнопку найма крестьян
    /// </summary>
    private void CheckInteractableButtonForHirePeasant()
    {
        if (_costPeasant > countSeed || _timeHirePeasant > 0)
        {
            _peasantCreateButton.interactable = false;
        }
        else 
        {
            _peasantCreateButton.interactable = true;
        }
    }

    /// <summary>
    /// Запускает отсчёт создания крестьянина и блокирует кнопку
    /// </summary>
    public void CreatePeasant()
    {
        countSeed -= _costPeasant;
        _peasantCreateButton.interactable = false;
        _timeHirePeasant = _timeForCraetePeasant;
    }

    /// <summary>
    /// Отсчитывает время создания крестьянина
    /// </summary>
    private void TimerCreatePeasant()
    {
        if (_timeHirePeasant > 0)
        {
            _timeHirePeasant -= Time.deltaTime;
            _peasantTimer.fillAmount = 1 - _timeHirePeasant / _timeForCraetePeasant;
        }
        else if (_timeHirePeasant > -1)
        {
            countPeasant += 1;
            _totalPeasant++;
            _timeHirePeasant = -2;
            _peasantTimer.fillAmount = 0;
        }
    }

    /// <summary>
    /// Выводит текст на экаран
    /// </summary>
    private void UpdateText()
    {
        resourText.text = countPeasant + "\n" + countWarrior + "\n\n" + countSeed;
        valueCountEnemy.text = _countEnemy.ToString();
    }

    /// <summary>
    /// Проверяет выйграл ли игрок, если да то сбрасывает параметры.
    /// </summary>
    private void ConditionWin()
    {
        if (countPeasant > _peasantWinningCondition && countSeed > _grainWinningCondition)
        {
            _playGame = false;
            countWarrior *= 0;
            ChangeScene(_winScene);
            _resultWinValue.text = _totalSeed + "\n" +
                _totalFoodEaten + "\n" +
                _totalPeasant + "\n" +
                _totalWarrior + "\n" +
                (_totalWave - 2);
            countPeasant = 5;
            countSeed = 0;
            countWarrior = 0;
            _countEnemy = 0;
            _totalFoodEaten = 0;
            _totalPeasant = 0;
            _totalSeed = 0;
            _totalWarrior = 0;
            _totalWave = 0;
            _timeHirePeasant = -2;
            _timeHireWarrior = -2;
            _peasantTimer.fillAmount = 0;
            _warriorTimer.fillAmount = 0;
        }
    }

    /// <summary>
    /// Проверяет проиграл ли игрок, если да то сбрасывает параметры, если нет увеличивает количество волн.
    /// </summary>
    private void ConditionLose()
    {
        if (countWarrior < 0 || countPeasant < 0)
        {
            _playGame = false;
            countWarrior *= 0;
            ChangeScene(_failScene);
            _resultLoseValue.text = _totalSeed + "\n" +
                _totalFoodEaten + "\n" +
                _totalPeasant + "\n" +
                _totalWarrior + "\n" +
                (_totalWave - 2);
            countPeasant = 5;
            countSeed = 0;
            countWarrior = 0;
            _countEnemy = -1;
            _totalFoodEaten = 0;
            _totalPeasant = 0;
            _totalSeed = 0;
            _totalWarrior = 0;
            _totalWave = -2;
            _timeHirePeasant = -2;
            _timeHireWarrior = -2;
            _peasantTimer.fillAmount = 0;
            _warriorTimer.fillAmount = 0;
        }
        else
        {
            _totalWave += 1;
        }
    }

    /// <summary>
    /// Случайным образом уменьшает количество воинов после атаки и проверяет условие проигрыша. Увеличивает количество врагов
    /// </summary>
    private void VillageUnderAttack()
    {
        if (_countEnemy > 0)
        {
            _strikeSword.Play();
            double deadWarrior = System.Math.Floor((float)(_countEnemy) / 2);
            countWarrior -= random.Next((int)deadWarrior, (_countEnemy) + 1);
        }
        ConditionLose();
        IncrementCountEnemy();
    }

    /// <summary>
    /// Увеличивает количество врагов
    /// </summary>
    private void IncrementCountEnemy()
    {
        if ((_totalWave + 1) % 3 == 0)
        {
            _countEnemy += 1;
        }
    }

    /// <summary>
    /// Проверяет тики игры
    /// </summary>
    private void CheckTick()
    {
        //Тик еды съедаемой войнами
        if (_eatingTimer.tick)
        {
            countSeed -= countWarrior * eatingWarrior;
            _totalFoodEaten += countWarrior * eatingWarrior;
        }

        // Тик еды добываемой крестьянами
        if (_seedTimer.tick)
        {
            countSeed += countPeasant * productionSeedPeasant;
            _totalSeed += countPeasant * productionSeedPeasant;
        }

        // Тик нападения на деревню
        if (_enemyTimer.tick)
        {
            VillageUnderAttack();
        }
    } 

    /// <summary>
    /// Перезапускает игру сначала
    /// </summary>
    public void RestartGame()
    {
        ChangeScene(_gameScene);
        _playGame = true;
    }

    /// <summary>
    /// Начинает игру, сбрасывая параметры и открывая инструкцию если надо
    /// </summary>
    public void StartGame()
    {
        if (_manualSceneCheck)
        {
            _manualSceneCheck = false;
            ChangeScene(_manualScene);
        }
        else
        {
            ChangeScene(_gameScene);
            countPeasant = 5;
            countSeed = 0;
            countWarrior = 0;
            _countEnemy = 0;
            _totalFoodEaten = 0;
            _totalPeasant = 0;
            _totalSeed = 0;
            _totalWarrior = 0;
            _totalWave = 0;
            _timeHirePeasant = -2;
            _timeHireWarrior = -2;
            _peasantTimer.fillAmount = 0;
            _warriorTimer.fillAmount = 0;
            _playGame = true;
        }
    }
}
