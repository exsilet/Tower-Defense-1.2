using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UI : MonoBehaviour
{
    [SerializeField] private Text _deathMonster;
    
    public Slider healthSlider;
    public Text health;

    public Slider enemyWavePercentSlider;

    float healthValue, enemyWaveValue, enemyValue;
    int countEnemyDeath;
    public float lerpSpeed = 1f;

    public Text pointTxt;
    public Text levelName;
    public Text ShopCoinText;

    private float _lerpSpeedEnemy = 0.05f;

    private void Start()
    {
        healthValue = 1;

        healthSlider.value = 1;
        enemyWavePercentSlider.value = 0;
        levelName.text = GlobalValue.levelPlaying.ToString();
    }

    private void Update()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, healthValue, lerpSpeed * Time.deltaTime);
        enemyWavePercentSlider.value = Mathf.Lerp(enemyWavePercentSlider.value, enemyValue, _lerpSpeedEnemy);

        pointTxt.text = GameManager.Instance.Point + "";
        ShopCoinText.text = GameManager.Instance.Point + "";
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        healthValue = Mathf.Clamp01(currentHealth / maxHealth);
        health.text = currentHealth + "/" + maxHealth;
    }

    public void UpdateEnemyWavePercent(float maxValue)
    {
        enemyWaveValue += maxValue;
        _deathMonster.text = countEnemyDeath + "/" + enemyWaveValue;
        //enemyWaveValue = Mathf.Clamp01(currentSpawn / maxValue);
    }


    public void UpdateEnemyDeath(int enemyDeath)
    {
        
        countEnemyDeath += enemyDeath;
        GameManager.Instance.Death = countEnemyDeath;
        Debug.Log(GameManager.Instance.Death);
        enemyValue = Mathf.Clamp01(countEnemyDeath / enemyWaveValue);
        _deathMonster.text = countEnemyDeath + "/" + enemyWaveValue;
    }
}
