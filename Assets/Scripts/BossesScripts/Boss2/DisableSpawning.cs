using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSpawning : MonoBehaviour
{    
    void OnTriggerEnter(Collider collider){

        if(collider.tag == "TargetPlayer"){
            transform.parent.GetComponent<Level2Boss>().enabledSpawning = false;

            foreach(Transform bomb in transform){
                if(bomb.tag == "Bomb2" || bomb.tag == "EnemyProjectile"){
                    Destroy(bomb);
                }
            }
        }
    }
}
