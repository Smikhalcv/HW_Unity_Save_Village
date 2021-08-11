using System;
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

    [SerializeField] private int countPeasant;
    [SerializeField] private int productionSeedPeasant;
    [SerializeField] private int _timeForCraetePeasant;
    [SerializeField] private int _costPeasant;
    [SerializeField] private int countWarrior;
    [SerializeField] private int eatingWarrior;
    [SerializeField] private int _timeForCraeteWarrior;
    [SerializeField] private int _costWarrior;
    [SerializeField] private int countSeed;
    [SerializeField] private Text resourText;
    [SerializeField] private Text valueCountEnemy;

    [SerializeField] private int _countEnemy;

    private static GameObject _currentScene;

    private static float _timeHireWarrior = -2;
    private static float _timeHirePeasant = -2;

    private static int _totalPeasant = 0;
    private static int _totalWarrior = 0;
    private static int _totalSeed = 0;
    private static int _totalWave = 0;
    private static int _totalFoodEaten = 0;

    private static bool _playGame = true;

    private static System.Random _random = new System.Random();
    [SerializeField] private SoundsManager _sounds;


    private void Start()
    {
        UpdateText();

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
    /// �������� ����� �����
    /// </summary>
    /// <param name="nextScene">����� �� ������� ���������� ��������</param>
    public void CallChangeScene(GameObject nextScene)
    {
        ChangeScene(nextScene);
    }

    /// <summary>
    /// ������ ����� �� ��������� � ��������� ����������
    /// </summary>
    /// <param name="nextScene">��������� �����</param>
    private void ChangeScene(GameObject nextScene)
    {
        _currentScene.SetActive(false);
        _currentScene = nextScene;
        _currentScene.SetActive(true);
    }

    /// <summary>
    /// ��������� ������ �������� ����� � ��������� ������ �������� ������, �������� ��������� ����� � ������
    /// </summary>
    public void CreateWarrior()
    {
        countPeasant -= 1;
        countSeed -= _costWarrior;
        _timeHireWarrior = _timeForCraeteWarrior;
        _warriorCreateButton.interactable = false;
    }

    /// <summary>
    /// ����������� ����� �������� �����
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
    /// ��������� ����� �� ����������� ������ ����� ������
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
    /// ��������� ����� �� ����������� ������ ����� ��������
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
    /// ��������� ������ �������� ����������� � ��������� ������
    /// </summary>
    public void CreatePeasant()
    {
        countSeed -= _costPeasant;
        _peasantCreateButton.interactable = false;
        _timeHirePeasant = _timeForCraetePeasant;
    }

    /// <summary>
    /// ����������� ����� �������� �����������
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
    /// ������� ����� �� ������
    /// </summary>
    private void UpdateText()
    {
        resourText.text = countPeasant + "\n" + countWarrior + "\n\n" + countSeed;
        valueCountEnemy.text = _countEnemy.ToString();
    }

    /// <summary>
    /// ��������� ������� �� �����, ���� �� �� ���������� ���������.
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
    /// ��������� �������� �� �����, ���� �� �� ���������� ���������, ���� ��� ����������� ���������� ����.
    /// </summary>
    private void ConditionLose()
    {
        if (countWarrior < 0 || countPeasant < 0)
        {
            _playGame = false;
            _resultLoseValue.text = _totalSeed + "\n" +
                _totalFoodEaten + "\n" +
                _totalPeasant + "\n" +
                _totalWarrior + "\n" +
                (_totalWave - 2);
            ChangeScene(_failScene);
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
    /// ��������� ������� ��������� ���������� ������ ����� ����� � ��������� ������� ���������. ����������� ���������� ������
    /// </summary>
    private void VillageUnderAttack()
    {
        if (_countEnemy > 0)
        {
            _sounds.AttackSound();
            double deadWarrior = System.Math.Floor((float)(_countEnemy) / 2);
            countWarrior -= _random.Next((int)deadWarrior, (_countEnemy) + 1);
        }
        ConditionLose();
        IncrementCountEnemy();
    }

    /// <summary>
    /// ����������� ���������� ������
    /// </summary>
    private void IncrementCountEnemy()
    {
        if ((_totalWave + 1) % 3 == 0)
        {
            _countEnemy += 1;
        }
    }

    /// <summary>
    /// ��������� ���� ����
    /// </summary>
    private void CheckTick()
    {
        //��� ��� ��������� �������
        if (_eatingTimer.Tick)
        {
            countSeed -= countWarrior * eatingWarrior;
            _totalFoodEaten += countWarrior * eatingWarrior;
        }

        // ��� ��� ���������� �����������
        if (_seedTimer.Tick)
        {
            countSeed += countPeasant * productionSeedPeasant;
            _totalSeed += countPeasant * productionSeedPeasant;
        }

        // ��� ��������� �� �������
        if (_enemyTimer.Tick)
        {
            VillageUnderAttack();
        }
    } 

    /// <summary>
    /// ������������� ���� �������
    /// </summary>
    public void RestartGame()
    {
        ChangeScene(_gameScene);
        _playGame = true;
    }

    /// <summary>
    /// �������� ����, ��������� ��������� � �������� ���������� ���� ����
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
