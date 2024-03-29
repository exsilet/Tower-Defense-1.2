using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace TD.Script.GUI
{
    public class GiftVideoAd : MonoBehaviour
    {
        [SerializeField] private Button _rewardButton;
        
        private void OnEnable()
        {
            YandeAdServices.RewardClosed += RewardClosed;
        }

        private void OnDisable()
        {
            YandeAdServices.RewardClosed -= RewardClosed;
        }

        public void WatchVideoAd(int id)
        {
            SoundManager.Click();
            YandexGame.RewVideoShow(id);
            _rewardButton.interactable = false;
        }

        private void RewardClosed()
        {
            _rewardButton.interactable = true;
        }
    }
}
