using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Livello2 : MonoBehaviour
{
    public GameObject dynamicPlayerHealthBar;
    public RawImage playerHealthBar;
    public RawImage bossHealthImg; // indicatore della vita del boss
    public RawImage bossIcon;
    public RawImage crosshair;
    public RectTransform gameOverPanel;
    public GameObject playerObject;
    public bool gameOver;
    public bool bossFight = false; // flag che determina se sta avvenendo una boss fight o meno
    public int playerLife; // vita del player
    public GameObject dynamicHealthBar;
    public Vector3 localScaleAux;
    public GameObject spawnPosition;
    public GameObject targetToSpawn;
    public int vitaBoss;
    public int fullVita;
    private int prev_vita;


    // Start is called before the first frame update
    void Start()
    {
        GetElements();
        Init();
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        ShowBossHealth(); // mostro o nascondo la barra della vita del boss
        CheckPlayerLife(); // controllo la vita del player
        UpdateHealthBar();
    }

    void ShowBossHealth(){
        // controllo se è in corso una bossfight
        if(bossFight){
            // rendo visibile la vita del boss
            bossHealthImg.enabled = true;
            dynamicHealthBar.SetActive(true);
            bossIcon.enabled = true;
        }
        else {
            // rendo invisibile la vita del boss
            bossHealthImg.enabled = false;
            dynamicHealthBar.SetActive(false);
            bossIcon.enabled = false;
        }
    }
    void UpdateHealthBar(){
        if(vitaBoss < prev_vita){
            UpdateHealthBar(vitaBoss);
        }
        prev_vita = vitaBoss;
    }
    public void UpdateHealthBar(float vitaBoss){        
        if(vitaBoss > 0 && bossFight){
            dynamicHealthBar.transform.localScale = new Vector3(vitaBoss * 0.81f, 9, 1);
        } else {
            bossFight = false;
            dynamicHealthBar.SetActive(false);
            bossIcon.enabled = false;
        }
    }
    public void UpdatePlayerHealthBarNegative(){
        dynamicPlayerHealthBar.transform.GetChild(--playerLife).gameObject.SetActive(false);
    }
    public void UpdatePlayerHealthBarPositive(){
        dynamicPlayerHealthBar.transform.GetChild(playerLife++).gameObject.SetActive(true);
    }

    private void CheckPlayerLife(){
        if(playerLife <= 0){
            gameOver = true;
            OnGameOver();
        }
    }
    public void OnGameOver(){
        print("Game Over");
        gameOverPanel.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
        playerHealthBar.gameObject.SetActive(false);
        dynamicHealthBar.gameObject.SetActive(false);
        dynamicPlayerHealthBar.gameObject.SetActive(false);
        bossHealthImg.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    void Init(){
        // playerObject.transform.position = playerSpawnPosition.transform.position;
        playerObject.transform.position = new Vector3(0f, 7.5f, 140f);
        bossFight = true;
        playerLife = 10;
        vitaBoss = 300;
        fullVita = 300;
        prev_vita = 300;
        localScaleAux = dynamicHealthBar.transform.localScale;
    }
    void GetElements(){
        playerObject = GameObject.Find("Player").gameObject;
        dynamicPlayerHealthBar = GameObject.Find("DynamicPlayerHealthBar");
        playerHealthBar = GameObject.Find("VitaPlayer").GetComponent<RawImage>();
        crosshair = GameObject.Find("Crosshair").GetComponent<RawImage>();
    }

    private IEnumerator Spawner(){
        Vector3 position;
        while(vitaBoss > 0){
            position = new Vector3(spawnPosition.transform.position.x + Random.Range(-240f, 240f),
            spawnPosition.transform.position.y + Random.Range(-240f, 240f),
            spawnPosition.transform.position.z + Random.Range(-240f, 240f));



            yield return new WaitForSeconds(0.5f);

            GameObject.Instantiate(targetToSpawn, position, playerObject.transform.rotation, transform);
        }
    }

    private IEnumerator AutoHeal(){
        yield return new WaitForSeconds(5f);

        if(vitaBoss < fullVita){
            vitaBoss += 10;

            if(vitaBoss > fullVita){
                vitaBoss = fullVita;
            }
        }
    }
}

