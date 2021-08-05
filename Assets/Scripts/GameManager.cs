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
    [SerializeField] private GameObject _sounds;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [SerializeField] private GameObject _firstScene;
    [SerializeField] private GameObject _winScene;
    [SerializeField] private GameObject _failScene;

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

    private static GameObject _currentScene;
    private static bool _muteFlag = true;
    private static float _timeHireWarrior = -2;
    private static float _timeHirePeasant = -2;



    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        _backgroundMusic.Play();
        _firstScene.SetActive(true);
        _currentScene = _firstScene;
        _warriorTimer.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Тик еды съедаемой войнами
        if (_eatingTimer.tick)
        {
            countSeed -= countWarrior * eatingWarrior;
        }

        // Тик еды добываемой крестьянами
        if (_seedTimer.tick)
        {
            countSeed += countPeasant * productionSeedPeasant;
        }

        TimerCreateWarrior();
        CheckInteractableButtonForHireWarrior();
        TimerCreatePeasant();
        CheckInteractableButtonForHirePeasant();
        ConditionWinOrFail();
        UpdateText();
    }

    /// <summary>
    /// Меняет сцену на указанную и закрывает следующую
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
    /// <param name="muteButton"></param>
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
        countSeed -= _costWarrior;
        _timeHireWarrior = _timeForCraeteWarrior;
        _warriorCreateButton.interactable = false;
    }

    /// <summary>
    /// Отсчитывает время создания война и разблокирует кнопку вызова
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
            _timeHireWarrior = -2;
            _warriorTimer.fillAmount = 0;
        }
    }


    /// <summary>
    /// Проверяет нужно ли блокировать кнопку найма воинов
    /// </summary>
    private void CheckInteractableButtonForHireWarrior()
    {
        if (_costWarrior > countSeed || _timeHireWarrior > 0)
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
        countSeed -= countPeasant;
        _peasantCreateButton.interactable = false;
        _timeHirePeasant = _timeForCraetePeasant;
    }

    /// <summary>
    /// Отсчитывает время создания крестьянина и разблокирует кнопку
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
    }

    /// <summary>
    /// Проверяет выйграл или проиграл игрок
    /// </summary>
    private void ConditionWinOrFail()
    {
        if (countPeasant > _peasantWinningCondition && countSeed > _grainWinningCondition)
        {
            ChangeScene(_winScene);
        }
        else if (countWarrior < 0)
        {
            ChangeScene(_failScene);
        }
    }
}
