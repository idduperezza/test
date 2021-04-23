using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Boss : MonoBehaviour
{

    public CharacterController mantaController;
    public GameObject bomba;
    public GameObject proiettile;
    public Vector3 spawnPosition;
    public float speed = 40f;
    public bool thisLevel;
    public bool enabledSpawning;
    public LevelManager manager;
    public float delayBombe = 10f;
    public float delayProiettili = 5f;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Level1Manager").GetComponent<LevelManager>();
        enabledSpawning = true;
        thisLevel = true;

        StartCoroutine(DelaySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckPos();
    }

    void Move(){
        if(thisLevel){
            spawnPosition = transform.Find("Teleport").transform.position;
            mantaController.Move(-transform.forward * speed * Time.deltaTime);
        }
    }

    void CheckPos(){
        if(transform.position.z >= -5f){
            manager.gameOver = true;
            manager.OnGameOver();
        }
    }

    private IEnumerator ShootPlayer(){
        while(enabledSpawning && thisLevel){
            GameObject.Instantiate(proiettile, new Vector3(spawnPosition.x, spawnPosition.y + 20f, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(proiettile, new Vector3(spawnPosition.x + 20f, spawnPosition.y, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(proiettile, new Vector3(spawnPosition.x - 20f, spawnPosition.y, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(proiettile, new Vector3(spawnPosition.x, spawnPosition.y - 20f, spawnPosition.z + 40), transform.rotation, transform);   

            yield return new WaitForSeconds(10f);
        } 
    }


    private IEnumerator ShootEarth(){
        while(enabledSpawning && thisLevel){
            GameObject.Instantiate(bomba, new Vector3(spawnPosition.x, spawnPosition.y + 50f, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(bomba, new Vector3(spawnPosition.x + 50f, spawnPosition.y, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(bomba, new Vector3(spawnPosition.x - 50f, spawnPosition.y, spawnPosition.z + 40), transform.rotation, transform);
            GameObject.Instantiate(bomba, new Vector3(spawnPosition.x, spawnPosition.y - 50f, spawnPosition.z + 40), transform.rotation, transform);

            yield return new WaitForSeconds(20f);
        }
    }

    public void DestroyBombs(){
        if(!thisLevel){
            foreach(Transform child in transform){
                if(child.tag == "Bomb2" || child.tag == "EnemyProjectile"){
                    Destroy(child.gameObject);
                }
            }
        }
    }

    private IEnumerator DelaySpawn(){
        yield return new WaitForSeconds(5f);

        StartCoroutine(ShootPlayer());
        StartCoroutine(ShootEarth());
    }

}
