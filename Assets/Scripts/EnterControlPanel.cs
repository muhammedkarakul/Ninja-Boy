using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterControlPanel : MonoBehaviour
{

    [SerializeField]
    private GameObject passwordTruePanel;

    [SerializeField]
    private GameObject passwordState;

    [SerializeField]
    private AudioSource enteranceApproved;

    [SerializeField]
    private AudioSource enteranceDenied;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player" && passwordState.activeSelf) {
            passwordTruePanel.SetActive(true);
            enteranceApproved.Play();
        } else {
            enteranceDenied.Play();
        }
    }

}
