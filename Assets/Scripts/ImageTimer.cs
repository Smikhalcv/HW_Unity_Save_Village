using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    public float maxTime;
    public bool tick;
    public float currentTime;

    private Image _imgTimer;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        _imgTimer = GetComponent<Image>();
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        tick = false;
        currentTime -= Time.deltaTime;
        _imgTimer.fillAmount = 1 - currentTime / maxTime;

        if (currentTime <= 0)
        {
            currentTime = maxTime;
            tick = true;
        }
    }
}
