using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public GameObject teleportAnimator;
    private GameObject player;
    private Quaternion zRotation = Quaternion.identity; // bho preso da internet 

    void Start(){
        player = GameObject.Find("Player");
    }
    void Update(){
        Rotate();
    }

    void OnTriggerEnter(Collider player){
        if(player.tag == "TargetPlayer"){
            transform.parent.GetComponent<Level2Boss>().thisLevel = false;
            transform.parent.GetComponent<Level2Boss>().DestroyBombs();
            teleportAnimator.SetActive(true);
            StartCoroutine(GoToNextLevel());
        }
    }

    IEnumerator GoToNextLevel(){

        yield return new WaitForSeconds(12.5f);

        SceneManager.LoadScene("BossFightManta");
    }

    void Rotate()
    {
        var lookAtTarget = player.transform.position - transform.position;
        var localForward = zRotation * Vector3.forward;
        var lookAt = Quaternion.LookRotation(lookAtTarget, localForward);
        var zDelta = Quaternion.AngleAxis(90f * Time.deltaTime, localForward);
        zRotation = zRotation * zDelta;
        transform.rotation = lookAt * zRotation;

        transform.LookAt(player.transform);
    }
}
