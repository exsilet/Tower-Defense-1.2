using UnityEngine;
using UnityEngine.UI;
using YG;

public class GiftVideoAd : MonoBehaviour
{
    public Text rewardedTxt;
    public GameObject button;
    public Text doubleArrow;
    public Text tripleArrow;

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
        if (id == 0)
        {
            AddMoney();
        }
        if (id == 1)
        {
            AddDoubleArrow();
        }
        if (id == 2)
        {
            AddTripleArrow();
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
        Debug.Log(GlobalValue.SavedCoins);
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddDoubleArrow()
    {
        GlobalValue.ItemDoubleArrow += 1;
        doubleArrow.text = GlobalValue.ItemDoubleArrow.ToString();
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }

    private void AddTripleArrow()
    {
        GlobalValue.ItemTripleArrow += 1;
        tripleArrow.text = GlobalValue.ItemTripleArrow.ToString();
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
    }
}
