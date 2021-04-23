using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerraColpita : MonoBehaviour
{
    public LevelManager manager; // gestore livello
    public float targetLife = 255; // vita del target

    void OnTriggerEnter(Collider collider){
        // controllo se la collisione è avvenuta con una bomba
        if(collider.transform.tag == "Bomb" || collider.transform.tag == "Bomb2"){
            
            // diminuisco la vita del target
            targetLife -= manager.bombDamage;
            Destroy(collider.gameObject);

        } else if(collider.transform.tag == "Target" || collider.transform.tag == "BombBody"){
            targetLife -= manager.bombDamage;

            if(collider.transform.parent){
                Destroy(collider.transform.parent.gameObject);
            } else {
                Destroy(collider.gameObject);
            }
        }

        // aggiorno il colore del target colpito da un nemico
        UpdateShieldColor(targetLife); 

        // controllo se la vita del target è maggiore di 0
        if(targetLife <= 0){
            // se è minore distruggo il gameobject
            Destroy(gameObject);
            // dminuisco le vite della terra
            manager.earthLife--;
            // e aggiorno il colore
            manager.UpdateEarthLife();
        }
    }

    void UpdateShieldColor(float valore){
        // gestisco i colori convertendo la vita del target in byte in modo da poterla passare come
        // parametro alla costruttore Color32
        byte red = Convert.ToByte(255 - valore);
        byte green = Convert.ToByte(valore);
        byte blue = 0;
        // cambio il colore
        gameObject.GetComponent<Renderer>().material.color = new Color32(red, green, blue, 80);
    }
}
