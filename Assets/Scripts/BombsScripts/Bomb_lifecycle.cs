using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_lifecycle : MonoBehaviour
{
    public GameObject corpo; // copro della bomba
    public Animator animator; // animator

    void Start(){
        animator.SetBool("Dead", false);
    }

    // Update is called once per frame
    void Update()
    {
        // controllo se il corpo della bomba non ha più figli (quindi non ha più target da sparare)
        // e se non è già stato distrutto
        if(corpo != null && corpo.transform.childCount == 0)
        {
            foreach(Transform child in transform) // controllo ogni figlio del gameObject per cercare il core e il corpo della bomba
            {
                if(child.tag == "BombBody") // quando trovo il corpo della bomba 
                {
                    animator.SetBool("Dead", true);
                    StartCoroutine(Die(child.gameObject));
                }

                if(child.tag == "Core") // quando trovo il core
                {
                    gameObject.transform.tag = "Target"; // per evitare un bug che avevamo di frequente tagghiamo tutto il gameobject come target
                    child.tag = "Target"; // lo taggo come target in modo da poter essere sparato
                }
            }
        }


        // se anche il core è stato colpito allora distruggo la bomba
        if(gameObject.transform.childCount == 0)
        {
            StartCoroutine(Die(gameObject));
        }
    }

    IEnumerator Die(GameObject oggettoDaDistruggere){
        yield return new WaitForSeconds(2f);

        Destroy(oggettoDaDistruggere); // distruggo la bomba
    }
}
