using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        //if (GameMode.Instance)
        //{
        //    rewardedTxt.text = AdsManager.Instance.rewardedWatchAd + "";
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //button.SetActive(allowShow && AdsManager.Instance && AdsManager.Instance.isRewardedAdReady());
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
        AddMoney();
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

    public void WatchVideoAd()
    {
        SoundManager.Click();
        //allowShow = false;
        //AdsManager.AdResult += AdsManager_AdResult;
        //AdsManager.Instance.ShowRewardedAds();
        //Invoke("AllowShow", 2);

        YandexGame.RewVideoShow(0);
    }

    private void AddMoney()
    {
        //AdsManager.AdResult -= AdsManager_AdResult;
        GlobalValue.SavedCoins += 300;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);

    }

    //void AllowShow()
    //{
    //    allowShow = true;
    //}
}
