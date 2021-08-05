using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float maxTime;
    public bool tick;

    private Image _imgTimer;
    private float _currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _imgTimer = GetComponent<Image>();
        _currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        tick = false;
        _currentTime -= Time.deltaTime;
        _imgTimer.fillAmount = 1 - _currentTime / maxTime;

        if (_currentTime <= 0)
        {
            _currentTime = maxTime;
            tick = true;
        }
    }
}
