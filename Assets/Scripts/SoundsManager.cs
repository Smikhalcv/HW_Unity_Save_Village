using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _strikeSword;
    [SerializeField] private GameObject _sounds;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;
    private static bool _muteFlag = true;

    /// <summary>
    /// Меняет изображение кнопки выключить звук
    /// </summary>
    /// <param name="muteButton">Кнопка для блокировки звука</param>
    public void ChangeSpriteSoundButton()
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
    /// запускает звук атаки на деревню
    /// </summary>
    public void AttackSound()
    {
        _strikeSword.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        _backgroundMusic.Play();
    }
}
