using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

	[SerializeField]
	private GameObject keyState;

	[SerializeField]
	private GameObject passwordState;

	[SerializeField]
	private GameObject doorOpen;

	[SerializeField]
	private AudioSource doorLockedSound;

	[SerializeField]
	private AudioSource doorOpenSound;

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Player" && (keyState.activeSelf || passwordState.activeSelf) ) {
			doorOpen.SetActive(true);
			collider.gameObject.SetActive(false);
			doorOpenSound.Play();
			SceneManager.LoadScene(1);
		} else if (collider.gameObject.tag == "Player") {
			doorLockedSound.Play();
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
