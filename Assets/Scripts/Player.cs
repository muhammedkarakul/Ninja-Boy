using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	// Player oyun nesnemize eklediğimiz Rigidbody2D bileşenini kod içinde temsil edecek Rigidbody2D nesnesini tanımlıyoruz.
	private Rigidbody2D playerRigidBody;

	// Player nesnemizin tüm animasyonları animator tarafından kontrol edilir.
	private Animator playerAnimator;

	// Aşağıdaki kod satırı altında tanımlanan değişkene inspector panelinden müdahale imkanı sunar.
	[SerializeField]
	// Player nesnemizin hızını tayin eden değişkenimiz.
 	private float velocity; 

 	[SerializeField]
	 // Anahtarın alınıp alınmadığı ile ilgili durumu tutan değişken.
 	private GameObject keyState;

	 [SerializeField]
	 private GameObject passwordState;

	// Altınlar ile temas(collision) durumunda bu ses dosyası oynatılır.
 	[SerializeField]
 	private AudioSource coinSound;

	// Anahtar ile temas(collision) durumunda bu ses dosyası oynatılır.
 	[SerializeField]
 	private AudioSource keySound;

	// Player nesnemizin baktığı yönü tayin eden değişkenimiz. true = right, false = left
 	private bool direction;

	// Skor ile ilgili değişkenler.
 	private int score;
 	public Text totalScore;

	// Atak animasyonunun durumunu tutan değişken.
	private bool attackState;

	// Kayma animasyonunun durumunu tutan değişken.
	private bool slideState;

	// Zıplama animasyonunun durumunu tutan değişken.
	private bool jumpState;

	private float jumpPower;

	private const float gravity = 9.8f;

	private bool isOnTheFloor;

    /*
	* Start foksiyonu sahne başlangıcında bir defaya mahsus kurulumlar için çalışır.
	*/
    void Start()
    {
		jumpPower = 0;

    	score = 0;

    	// Oyun başladığında karakterimiz sağa baktığı için yön değişkenini true olarak atıyoruz.
    	direction = true;

    	// Player nesnemize unity içinden eklediğimiz Rigidbody2D bileşenini kod içinde oluşturduğumuz Rigidbody2D nesnemize bağlıyoruz.
    	playerRigidBody = GetComponent<Rigidbody2D>();

    	playerAnimator = GetComponent<Animator>();
    }

	/*
	* Sürekli kontrol edilmesi gereken(Klavyeden bir tuşa basıldı mı? vb.) komutlar bu update fonksiyonu içinde yer alır.
	*/
	void Update() {
		checkKeyboardControls();
	}

	/*
	* Fizik ile ilgili kontroller bu fixed update fonksiyonu içinde yer almalıdır. Böylece daha gerçekçi fizik davranışları elde ederiz.
	*/
    void FixedUpdate()
    {
    	// Player nesnemizi yatayda hareket ettireceğimizi belirtiyoruz. Bu bize kontrol olarak 
        float horizontal = Input.GetAxis("Horizontal");
		
		basicMovements(horizontal);
		
        changeDirection(horizontal);

		playerRigidBody.gravityScale = jumpPower;

		if (jumpPower > gravity) {
			jumpPower -= playerRigidBody.mass;
		} else {
			jumpPower = gravity;
		}
		
		resetValues();
    }

	/*
	* Klavye ile ilgili kontroller(Tuşa basıldı mı? vb.) bu metodda yapılır. 
	*/
	private void checkKeyboardControls() {
		// Klavyeden "T" tuşuna basılırsa attackState değişkeni true olur. Tersi durumda false olur.
		attackState = Input.GetKeyDown(KeyCode.T);

		// Klavyeden "Y" tuşuna basılırsa slideState değişkeni true olur. Tersi durumda false olur.
		slideState = Input.GetKeyDown(KeyCode.Y);

		// Klavyeden "Y" tuşuna basılırsa slideState değişkeni true olur. Tersi durumda false olur.
		jumpState = Input.GetKeyDown(KeyCode.U);
	}

	/*
	* Nesnemiz ile ilgili temel hareketler(Koşma, Atak, Zıplama vb.) bu metod sayesinde nesnesinemize yaptırılır.
	*/
    private void basicMovements(float horizontal) {

		if (!this.playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {

			// Rigidbody2D nesnesinin hız değerini değiştirerek player nesnemizin hareket etmesini sağlıyoruz.
			playerRigidBody.velocity = new Vector2(horizontal * velocity, playerRigidBody.velocity.y);

			// Player nesnemizin animator eklentisindeki playerVelocity değişkeninin değerini değiştiriyoruz. Bu değer karakterin animasyonlarını etkiler.
			playerAnimator.SetFloat("playerVelocity", Mathf.Abs(horizontal));

			// Bu if koşulu attackState değişkeni true olduğu sürece çalışır.
			if (attackState) {
				// Player nesnemizin animator eklentisindeki attackState değişkeni tetiklenir. Bu değer değişince karakterimiz vurma animasyonu yapar.
				playerAnimator.SetTrigger("attackState");
				playerRigidBody.velocity = Vector2.zero;
			}

			// Bu if koşulu slideState değişkeni true olduğu sürece çalışır.
			if (slideState) {
				// Player nesnemizin animator eklentisindeki slideState değişkeni tetiklenir. Bu değer değişince karakterimiz kayma animasyonu yapar.
				playerAnimator.SetTrigger("slideState");
			}

			// Bu if koşulu slideState değişkeni true olduğu sürece çalışır.
			if (jumpState && isOnTheFloor) {

				isOnTheFloor = false;

				jumpPower = - playerRigidBody.gravityScale * velocity;

				// Player nesnemizin animator eklentisindeki slideState değişkeni tetiklenir. Bu değer değişince karakterimiz kayma animasyonu yapar.
				playerAnimator.SetTrigger("jumpState");
			}

		}
    }

	/*
	* Bu metod karakterin gittiği yöne doğru görselininde dönmesini sağlar. Görsel üstünde transform işlemi uygulanır.
	* - horizontal: Player nesnesinin gittiği yön.
	*/
    private void changeDirection(float horizontal) {
    	if (horizontal > 0 && direction == false || horizontal < 0 && direction == true) {
    		direction = !direction;
    		Vector3 currentDirecton = transform.localScale;
    		currentDirecton.x *= -1;
    		transform.localScale = currentDirecton;
    	}
    }

	/*
	* Bu metod çarpışma durumlarında çalışır. 
	* - collision: Çarpışma durumlarında çarpışılan nesne ile ilgili bilgi almamızı sağlayan çarpışma kontrol nesnesi.
	*/
    private void OnCollisionEnter2D(Collision2D collision) {
    	if (collision.gameObject.tag == "Coin") {
    		collision.gameObject.SetActive(false);
    		score = score + 100;
    		updateScore(score);
    		coinSound.Play();
    	}

    	if (collision.gameObject.tag == "Key") {
    		collision.gameObject.SetActive(false);
    		keyState.SetActive(true);
    		keySound.Play();
    	}

		if (collision.gameObject.tag == "Floor") {
			isOnTheFloor = true;
		}

		if (collision.gameObject.name == "Box" && this.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerSlide")) {
			collision.gameObject.SetActive(false);
			passwordState.SetActive(true);
		}
    }

	/* 
	* Score değerini text nesnesine yazar.
	*/
    private void updateScore(int count) {
    	totalScore.text = "Score: " + count.ToString();
    }

	/*
	* Bu metod değerleri ilk haline döndürür.
	*/
	private void resetValues() {
		
	}
}