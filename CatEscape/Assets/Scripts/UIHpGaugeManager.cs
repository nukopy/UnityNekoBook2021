using System.Collections;
using System.Collections.Generic;
using CatEscape;
using UnityEngine;
using UnityEngine.UI;

public class UIHpGaugeManager : MonoBehaviour
{
    // constants
    private const float HpGaugeYellowThreshold = 0.5f;
    private const float HpGaugeYellowAlpha = 0.5f;
    private const float HpGaugeRedThreshold = 0.1f;
    private const float HpGaugeRedAlpha = 0.1f;

    // members
    private GameObject _hpGauge;
    private GameObject _player;

    void Start()
    {
        this._player = GameObject.Find("Player");
        this._hpGauge = GameObject.Find("HpGauge");
    }
    
    void Update()
    {
        this.UpdateHpGauge();
    }

    /// <summary>
    /// Player の体力を HP ゲージを画面に反映させる
    /// </summary>
    void UpdateHpGauge()
    {
        var pc = this._player.GetComponent<PlayerController>();
        float hpRatio = pc.GetPlayerHpRatio();
        Image img = this._hpGauge.GetComponent<Image>();
        img.fillAmount = hpRatio;
        
        // HP ゲージの色の透明度を変化させる
        Color color = img.color;
        if (pc.GetPlayerHpRatio() <= HpGaugeYellowThreshold)
        {
            color.a = HpGaugeYellowAlpha;
            img.color = color;
        } else if (pc.GetPlayerHpRatio() <= HpGaugeRedThreshold)
        {
            color.a = HpGaugeRedAlpha;
            img.color = color;
        }
    }
}
