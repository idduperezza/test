using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Player_shooting : MonoBehaviour
{
    public GameObject eyes; // rappresentano gli occhi del personaggio (ci va associata la Main Camera)
    public GameObject rifle;
    public GameObject player;
    LineRenderer laser;
    public Material laserMaterial;
    public Animator animator;

    void Start(){
        laser = GetComponent<LineRenderer>();
        laser.positionCount = 2;
        laser.material = laserMaterial;
        laser.startWidth = 0.25f;
        laser.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; // dichiaro la variabile in cui salvare i dati dell'oggetto colpito dal raycast

        // controllo se il player ha premuto mouse 1 (il bottone per sparare)
        if(Input.GetButtonDown("Fire1"))
        {
            // triggero l'animazione della sparata se non sto mirando
            if(player.GetComponentInChildren<MouseLook>().scoped == false){
                animator.SetTrigger("Shoot");
            }

            // restituisce true se ho intersecato qualcosa, false altrimenti. Salvo i dati di ciò che ho intersecato
            // nella variabile hit tramite la keyword out
            if(Physics.Raycast(eyes.transform.position, eyes.transform.forward, out hit)){
                
                // sparo un laser
                laser.enabled = true;
                laser.SetPosition(0, rifle.transform.position);
                laser.SetPosition(1, hit.point + hit.normal);

                // controllo se ciò che ho colpito è un target (punto debole delle bombe)
                if(hit.collider.tag == "Target" || hit.collider.tag == "EnemyProjectile")
                {
                    player.GetComponent<Player_movement>().AddBoost();

                    // controllo se ho colpito tutti i punti deboli dell'asteroide
                    if(hit.transform.tag == "Asteroide" && hit.transform.childCount == 1){
                        player.GetComponent<PlayerLifeManager>().Heal();
                    }

                    if(hit.transform.parent.name == "Level2Manager"){
                        GameObject.Find("Level2Manager").GetComponent<Livello2>().vitaBoss -= 2;
                    }

                    // distruggo l'oggetto colpito (solo se è un target)
                    Destroy(hit.collider.gameObject);
                }

                // se ho colpito il punto debole del boss gli tolgo vita
                if(hit.transform.tag == "WeakPoint")
                {
                    player.GetComponent<Player_movement>().AddBoost();
                    hit.transform.parent.GetComponent<Level1Boss>().vitaBoss -= 2;
                    //Destroy(hit.transform.gameObject); // distruggo l'oggetto colpito (solo se è un target)
                }

                // se ho colpito un asteroide lo spingo via
                if(hit.collider.tag == "Asteroide"){
                    hit.rigidbody.AddForce(transform.forward * 300f);
                }

                print("ho colpito: " + hit.collider.tag);
            }
        } else {
            laser.enabled = false;
        }
    }


}
