using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    [SerializeField] private float maxTime;
    private bool tick;
    private float currentTime;

    private Image _imgTimer;
    
    public bool Tick
    {
        get { return this.tick; }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        _imgTimer = GetComponent<Image>();
        currentTime = maxTime;
    }

    // Update is called once per frame
    private void Update()
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
