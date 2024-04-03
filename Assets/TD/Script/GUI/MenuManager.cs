using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

public class MenuManager : MonoBehaviour, IListener
{
    public static MenuManager Instance;
    public GameObject Shoot;
    public GameObject StartUI;
    public GameObject UI;
    public GameObject VictoryUI;
    public GameObject FailUI;
    public GameObject PauseUI;
    public GameObject LoadingUI;
    public GameObject ShopUI;

    [Header("Sound and Music")]
    public Image soundImage;
    public Image musicImage;
    public Sprite soundImageOn, soundImageOff, musicImageOn, musicImageOff;

    UI_UI uiControl;
    private bool _activeMap = true;

    private void Awake()
    {
        Instance = this;

        Shoot.SetActive(false);
        StartUI.SetActive(false);
        UI.SetActive(false);
        VictoryUI.SetActive(false);
        FailUI.SetActive(false);
        PauseUI.SetActive(false);
        LoadingUI.SetActive(false);
        ShopUI.SetActive(false);

        uiControl = gameObject.GetComponentInChildren<UI_UI>(true);
    }

    IEnumerator Start()
    {
        soundImage.sprite = GlobalValue.isSound ? soundImageOn : soundImageOff;
        musicImage.sprite = GlobalValue.isMusic ? musicImageOn : musicImageOff;
        if (!GlobalValue.isSound)
            SoundManager.SoundVolume = 0;
        if (!GlobalValue.isMusic)
            SoundManager.MusicVolume = 0;

        StartUI.SetActive(true);

        yield return new WaitForSeconds(1);
        Shoot.SetActive(true);
        StartUI.SetActive(false);
        UI.SetActive(true);

        GameManager.Instance.StartGame();
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        uiControl.UpdateHealthbar(currentHealth, maxHealth);
    }

    public void UpdateEnemyWavePercent(float maxValue)
    {
        uiControl.UpdateEnemyWavePercent(maxValue);
    }
    
    public void UpdateEnemyDeath(int countEnemyDeath)
    {
        uiControl.UpdateEnemyDeath(countEnemyDeath);
    }

    public void Pause()
    {
        SoundManager.PlaySfx(SoundManager.Instance.soundPause);
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            UI.SetActive(false);
            PauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            UI.SetActive(true);
            PauseUI.SetActive(false);
        }
    }

    public void IPlay()
    {
       
    }

    public void ISuccess()
    {
        StartCoroutine(VictoryCo());
    }

    IEnumerator VictoryCo()
    {
        Shoot.SetActive(false);
        UI.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        VictoryUI.SetActive(true);
    }

    public void IPause()
    {
      
    }

    public void IUnPause()
    {
        
    }

    public void IGameOver()
    {
        StartCoroutine(GameOverCo());
    }

    IEnumerator GameOverCo()
    {
        Shoot.SetActive(false);
        UI.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        FailUI.SetActive(true);
    }

    public void IOnRespawn()
    {
        
    }

    public void IOnStopMovingOn()
    {
        
    }

    public void IOnStopMovingOff()
    {
       
    }

    //private void OnEnable()
    //{ 
    //    YandexGame.OpenFullAdEvent += OpenFullVideo;
    //    YandexGame.CloseFullAdEvent += CloseFullVideo;
    //}
    //
    //private void OnDisable()
    //{
    //    YandexGame.OpenFullAdEvent -= OpenFullVideo;
    //    YandexGame.CloseFullAdEvent -= CloseFullVideo;
    //}
    //
    //private void OpenFullVideo()
    //{
    //    AudioListener.volume = 0;
    //    Time.timeScale = 0;      
    //}
    //
    //private void CloseFullVideo()
    //{       
    //    Time.timeScale = 1;
    //    AudioListener.volume = 1;
    //}

    #region Music and Sound
    public void TurnSound()
    {
        GlobalValue.isSound = !GlobalValue.isSound;
        soundImage.sprite = GlobalValue.isSound ? soundImageOn : soundImageOff;

        SoundManager.SoundVolume = GlobalValue.isSound ? 1 : 0;
    }

    public void TurnMusic()
    {
        GlobalValue.isMusic = !GlobalValue.isMusic;
        musicImage.sprite = GlobalValue.isMusic ? musicImageOn : musicImageOff;

        SoundManager.MusicVolume = GlobalValue.isMusic ? SoundManager.Instance.musicsGameVolume : 0;
    }
    #endregion

    #region Load Scene
    public void LoadHomeMenuScene()
    {
        SoundManager.Click();
        
        StartCoroutine(LoadAsynchronously("Menu"));
        YandexGame.FullscreenShow();
    }
    
    public void OpenShop(bool open)
    {
        ShopUI.SetActive(open);
    }
    
    public void RestarLevel()
    {
        SoundManager.Click();
        
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().name));
        YandexGame.FullscreenShow();
    }

    public void LoadNextLevel()
    {
        SoundManager.Click();
        GlobalValue.levelPlaying++;
        GlobalValue.isMap = _activeMap;
        StartCoroutine(LoadAsynchronously("Menu"));
        
        //string nextLevel = "Lv" + GlobalValue.levelPlaying;
        //StartCoroutine(LoadAsynchronously(nextLevel));
        YandexGame.FullscreenShow();
    }

    [Header("Load scene")]
    public Slider slider;
    public Text progressText;
    IEnumerator LoadAsynchronously(string name)
    {
        LoadingUI.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = (int)progress * 100f + "%";
            //			Debug.LogError (progress);
            yield return null;
        }
    }
    #endregion

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
