using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    private BoxCollider2D playerCollider;

//    [SerializeField]
    private BoxCollider2D collisionCollider;

    // Start is called before the first frame update
    void Start() {
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        collisionCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player") {
            Physics2D.IgnoreCollision(collisionCollider, playerCollider, true);
        } 
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.name == "Player") {
            Physics2D.IgnoreCollision(collisionCollider, playerCollider, false);
        }
    }
}
