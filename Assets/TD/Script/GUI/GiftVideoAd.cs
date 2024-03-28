﻿using UnityEngine;
using UnityEngine.UI;
using YG;

public class GiftVideoAd : MonoBehaviour
{
    public Text rewardedTxt;
    public GameObject button;

    private Button adBtn;
    //bool allowShow = true;
    // Start is called before the first frame update
    void Start()
    {
        adBtn = button.GetComponent<Button>();
    }

    private void OnEnable()
    {
        YandexGame.OpenVideoEvent += OpenVideoReward;
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.CloseVideoEvent += CloseVideoReward;
    }

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable()
    {
        YandexGame.OpenVideoEvent -= OpenVideoReward;
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.CloseVideoEvent -= CloseVideoReward;
    }

    // Подписанный метод получения награды
    void Rewarded(int id)
    {
        switch (id)
        {
            case 0:
                AddMoney();
                break;
            case 1:
                AddDoubleArrow();
                break;
            case 2:
                AddTripleArrow();
                break;
        }
    }

    private void OpenVideoReward()
    {
        adBtn.interactable = false;
        Time.timeScale = 0;
    }

    private void CloseVideoReward()
    {
        adBtn.interactable = true;
        Time.timeScale = 1;
    }

    public void WatchVideoAd(int id)
    {
        SoundManager.Click();
        YandexGame.RewVideoShow(id);
    }

    private void AddMoney()
    {
        //AdsManager.AdResult -= AdsManager_AdResult;
        GlobalValue.SavedCoins += 300;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddDoubleArrow()
    {
        GlobalValue.ItemDoubleArrow += 1;
        rewardedTxt.text = "x" + GlobalValue.ItemDoubleArrow;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddTripleArrow()
    {
        GlobalValue.ItemTripleArrow += 1;
        rewardedTxt.text = "x" + GlobalValue.ItemTripleArrow;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }
}
