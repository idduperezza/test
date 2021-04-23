using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float delay;
    public float rangeX;
    public float negRangeX;
    public float rangeY;
    public float negRangeY;
    public float rangeZ;
    public float negRangeZ;
    public GameObject oggettoDaSpawnare;
    public GameObject posizioneRandomSpawn;
    public LevelManager manager;

    public bool enabledSpawning;

    // Start is called before the first frame update
    void Start()
    {
        enabledSpawning = true;
        StartCoroutine("Spawner");
    }

    public IEnumerator Spawner()
    {
        Vector3 spawnPosition;

        while(manager.earthLife > 0)
        {
            if(enabledSpawning){
                spawnPosition = new Vector3(
                posizioneRandomSpawn.transform.position.x + Random.Range(negRangeX, rangeX),
                posizioneRandomSpawn.transform.position.y + Random.Range(negRangeY, rangeY),
                posizioneRandomSpawn.transform.position.z + Random.Range(negRangeZ, rangeZ));

                /**
                GameObject.Instantiate(Oggetto da spawnare, posizione, rotazione, padre dell'oggetto da spawnare);
                **/
                GameObject.Instantiate(oggettoDaSpawnare, spawnPosition, transform.rotation, posizioneRandomSpawn.transform);
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
