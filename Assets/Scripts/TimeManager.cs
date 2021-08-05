using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool _pauseTime = false;
    [SerializeField]
    private bool _speedTime = false;
    [SerializeField]
    private float _normalStepTime = 1;
    [SerializeField]
    private float _speedStepTime = 5;

    /// <summary>
    /// ������ ��� ������� ����� � ����
    /// </summary>
    public void PauseGame()
    {
        if (_pauseTime)
        {
            _pauseTime = false;
            _speedTime = false;
            Time.timeScale = _normalStepTime;
        }
        else
        {
            _pauseTime = true;
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// �������� ��� ���������� ����� ����� � ����
    /// </summary>
    public void SpeedTime()
    {
        if (_speedTime)
        {
            _speedTime = false;
            _pauseTime = false;
            Time.timeScale = _normalStepTime;
        }
        else
        {
            _speedTime = true;
            Time.timeScale = _speedStepTime;
        }
    }
}
