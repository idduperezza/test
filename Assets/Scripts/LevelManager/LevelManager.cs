using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject dynamicPlayerHealthBar;
    public RawImage playerHealthBar;
    public RawImage bossHealthImg; // indicatore della vita del boss
    public RawImage bossIcon;
    public RawImage crosshair;
    public RectTransform gameOverPanel;
    public GameObject playerObject;
    public bool gameOver;
    public RawImage earthLifeImg; // indicatore della vita della terra
    public GameObject earthShield; // scudo della terra
    public bool bossFight = false; // flag che determina se sta avvenendo una boss fight o meno
    public int earthLife = 3; // vita della terra
    public int playerLife; // vita del player
    public GameObject asteroide; // asteroide
    public int numero_asteroidi = 1000; // numero asteroidi da spawnare 
    public GameObject spawnerAsteroidi; // punto di partenza degli asteroidi
    public float bombDamage = 25.5f; // danno della bomba
    public GameObject dynamicHealthBar;
    public Vector3 localScaleAux;
    public GameObject mantaBoss;
    private bool mantaSpawned;


    // Start is called before the first frame update
    void Start()
    {
        mantaSpawned = false;
        bossFight = false;
        playerLife = 10;
        dynamicHealthBar.SetActive(false);
        localScaleAux = dynamicHealthBar.transform.localScale;
        SpawnAsteroidi(); // spawno gli asteroidi
    }

    // Update is called once per frame
    void Update()
    {
        ShowBossHealth(); // mostro o nascondo la barra della vita del boss
        CheckPlayerLife(); // controllo la vita del player
        CheckEarthLife(); // controllo la vita della terra
        NextLevel(); // controllo se è il caso di passare al prossimo livello
    }
    public void NextLevel(){

        if(GameObject.FindGameObjectsWithTag("EdroBoss").Length == 0){
            if(!mantaSpawned){
                Destroy(spawnerAsteroidi.transform.Find("Wall5").gameObject);
                StartCoroutine(SpawnMantaBoss());
                mantaSpawned = true;
            }
        }
    }
    IEnumerator SpawnMantaBoss(){
        yield return new WaitForSeconds(5f);

        GameObject.Instantiate(mantaBoss, new Vector3(0, 0, -4000f), new Quaternion(0, 180, 0, 0));        
    }

    public void UpdateEarthLife(){
        // controllo la vita della terra
        switch(earthLife){
            case 3: // se la terra ha ancora tutta la vita lascio tutto verde
                earthLifeImg.GetComponent<RawImage>().color = new Color32(255, 255, 255, 255);
                earthShield.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 9);
                break;
            case 2: // se la terra ha perso una vita tendo un po' più sul rosso
                earthLifeImg.GetComponent<RawImage>().color = new Color32(255, 127, 127, 255);
                earthShield.GetComponent<Renderer>().material.color = new Color32(127, 127, 0, 9);
                break;
            case 1: // se la terra è stata colpita già 2 volte faccio tutto rosso
                earthLifeImg.GetComponent<RawImage>().color = new Color32(255, 0, 0, 255);
                earthShield.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 9);
                break; 
        }
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
    void SpawnAsteroidi(){
        // spawno gli asteroidi
        for(int i = 0; i < numero_asteroidi; i++){
            // randomizzo una posizione per far spawnare gli asteroidi
            Vector3 asteroideSpawnPosition = new Vector3(spawnerAsteroidi.transform.position.x + Random.Range(-spawnerAsteroidi.transform.localScale.x * 4, spawnerAsteroidi.transform.localScale.x * 4),
            spawnerAsteroidi.transform.position.y + Random.Range(-spawnerAsteroidi.transform.localScale.y * 4, spawnerAsteroidi.transform.localScale.y * 4),
            spawnerAsteroidi.transform.position.z + Random.Range(-spawnerAsteroidi.transform.localScale.z * 4, spawnerAsteroidi.transform.localScale.z -40)); 
            
            // randomizzo la dimensione dell'asteroide da spawnare
            asteroide.transform.localScale = Vector3.one * Random.Range(1, 30);
            
            // spawno l'asteroide
            GameObject.Instantiate(asteroide, asteroideSpawnPosition, Random.rotation); 
        }
    }
    private void CheckPlayerLife(){
        if(playerLife <= 0){
            gameOver = true;
            OnGameOver();
        }
    }
    private void CheckEarthLife(){
        if(earthLife <= 0){
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
        earthLifeImg.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
}

