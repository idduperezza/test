using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullComportamento : MonoBehaviour
{
    private GameObject player;
    private CharacterController controller;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        InseguiPlayer();
    }

    private void InseguiPlayer(){
        controller.Move(transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime);
    }
}
