using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead {  get; set; }
   // public GameOverManager gameOverManager;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

   

    private Slider healthSlider;
    public int currHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private float waitToLoadTime = 1f;

    const string HEALTH_SLIDER_TEXT = "HeartSlider";
    const string TOWN_TEXT = "Scene1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currHealth = maxHealth;
       // currHealth = 1;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
         EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if( enemy)
        {
            TakeDamage(1, other.transform);
            
        }
    }

    public void HealPlayer()
    {
        if(currHealth < maxHealth)
        {
            currHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage (int damageAmount , Transform hitTransform)
    {
        if(!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform.transform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currHealth -= damageAmount;
        StartCoroutine(damageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    public void CheckIfPlayerDeath()
    {
        if(currHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            GameObject otherObject = GameObject.Find("EconomyManager");
           // GameObject otherObject1 = GameObject.Find("Stamina");
            if (otherObject != null)
            {
                EconomyManager playerCoin = otherObject.GetComponent<EconomyManager>();
                if (playerCoin != null)
                {
                    playerCoin.currentGold = -1;
                    playerCoin.UpdateCurrentGold();
                    Debug.Log("Health set to: " + playerCoin.currentGold);
                }  
                Stamina playerStamina = gameObject.GetComponent<Stamina>();
                if (playerStamina != null)
                {
                    playerStamina.CurrentStamina = playerStamina.startingStamina;
                    playerStamina.UpdateStaminaImages();
                    Debug.Log("Health set to: " + playerStamina.CurrentStamina);
                }
            }
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            UIFade.Instance.FadeToBack();
            StartCoroutine(DeathLoadSceneRoutine());
            
        }
    }

    public IEnumerator DeathLoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
        UIFade.Instance.FadeToClear();

    }

    private IEnumerator damageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent < Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currHealth;
    }
}
