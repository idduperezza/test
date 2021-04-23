using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeManager : MonoBehaviour
{
    public LevelManager manager;
    public Livello2 manager2;

    void Update(){
        if(!manager){
            manager2 = GameObject.Find("Level2Manager").GetComponent<Livello2>();
        }
    }

    void OnCollisionEnter(Collision collider){
        if(manager){
            manager.UpdatePlayerHealthBarNegative();
        } else {
            manager2.UpdatePlayerHealthBarNegative();
        }
        
        if(collider.transform.tag == "EnemyProjectile"){
            Destroy(collider.gameObject);
        }
    }

    public void Heal(){
        if(manager){
            if(manager.playerLife < 10){ 
                manager.UpdatePlayerHealthBarPositive();
            }
        } else {
            if(manager2.playerLife < 10){
                manager2.UpdatePlayerHealthBarPositive();
            }
        }

    }
}
