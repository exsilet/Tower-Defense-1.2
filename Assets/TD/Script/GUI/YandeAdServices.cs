using System;
using UnityEngine;
using YG;

public class YandeAdServices : MonoBehaviour
{
    public static Action RewardClosed;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
        YandexGame.OpenVideoEvent += OpenVideoReward;
        YandexGame.CloseVideoEvent += CloseVideoReward;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
        YandexGame.OpenVideoEvent -= OpenVideoReward;
        YandexGame.CloseVideoEvent -= CloseVideoReward;
    }

    private void Rewarded(int id)
    {
        switch (id)
        {
            case 1:
                AddMoney();
                break;
            case 2:
                AddDoubleArrow();
                break;
            case 3:
                AddTripleArrow();
                break;
        }
    }

    private void OpenVideoReward()
    {
        Time.timeScale = 0;
    }

    private void CloseVideoReward()
    {
        RewardClosed?.Invoke();
        Time.timeScale = 1;
    }

    private void AddMoney()
    {
        GlobalValue.SavedCoins += 600;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddDoubleArrow()
    {
        GlobalValue.ItemDoubleArrow += 1;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddTripleArrow()
    {
        GlobalValue.ItemTripleArrow += 1;
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }
}