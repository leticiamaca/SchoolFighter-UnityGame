using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public Animator transition;
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 1f;
    public float currentSpeed;

    public Vector2 playerDirection;

    private bool isWalking;
    private Animator playerAnimator;

    // Player olhando para direita
    private bool playerFacingRight = true;

    //Variavel contadora
    private int punchCount;

    //Tempo de ataque
    private float timeCross = 0.75f;

    private bool comboControl;


    //Variavel de vida máxima para os elementos UI / Propriedades para a UI
    public int maxHealth; //Tem que ser public porque a UIManager está usado essa variavel
    public int currentHealth;
    public Sprite playerImage;

    //SFX do Player
    private AudioSource playerAudioSource;
    public AudioClip jabSound;
    //public AudioClip crossSound;
    //public AudioClip deathSound;


    // Indica se o player esta morto
    private bool isDead;

    void Start()
    {
        //Obtem e inicializa as propriedades do RigiBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Obtem e inicializa as propriedades do animator
        playerAnimator = GetComponent<Animator>();

        currentSpeed = playerSpeed;

        //Iniciar a vida do player
        currentHealth = maxHealth;

        //Inicializando o som do player
        playerAudioSource = GetComponent<AudioSource>();

    }


    private void Update()
    {



        PlayerMove();
        UpdateAnimator();



        if (Input.GetKeyDown(KeyCode.X))
        {
            if (punchCount < 2)
            {
                PlayerJab();
                punchCount++;

                if (!comboControl)
                {
                    StartCoroutine(CrossController());
                }
            }
            else if (punchCount >= 2)
            {
                PlayerCross();
                punchCount = 0;
            }

            StopCoroutine(CrossController());

        }
    }

    // Fixed Update geralmente é utilizada para implementação de física no jogo, por ter uma execução padronizada em diferentes dispositivos
    private void FixedUpdate()
    {
        // Verificar se o Player está em movimento
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        //O que da o movimento para o player
        //playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);
        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);

    }

    void PlayerMove()
    {
        //Pega a entrada do jogador e cria um Vector2 para usar no playerDirection
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o Player se movimenta para ESQUERDA e está olhando para a DIREITA
        if (playerDirection.x < 0 && playerFacingRight)
        {
            Flip();
        }

        // Se o Player se movimenta para DIREITA e está olhando para a ESQUERDA
        else if (playerDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }

    }

    void UpdateAnimator()
    {
        // Definir o valor do parâmetro do animator, igual à propriedade isWalking
        playerAnimator.SetBool("isWalking", isWalking);
    }

    void Flip()
    {
        // Vai girar o sprite do player em 180 graus

        // Inverter o valor da variável playerfacingRight
        playerFacingRight = !playerFacingRight;

        // Girar o sprite do player em 180 no eixo Y
        // X, Y, Z
        transform.Rotate(0, 180, 0);

    }

    void PlayerJab()
    {
        //Acessa a animação do Jab
        //Ativa o gatilho de ataque jab
        playerAnimator.SetTrigger("isJab");

        //Definir o SFX a ser reproduzido
        playerAudioSource.clip = jabSound;

        //Executar o SFX
        playerAudioSource.Play();
    }

    void PlayerCross()
    {
        playerAnimator.SetTrigger("isCross");
        //Executar o SFX
        playerAudioSource.Play();
    }

    IEnumerator CrossController()
    {
        comboControl = true;
        yield return new WaitForSeconds(timeCross);
        punchCount = 0;
        comboControl = false;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = playerSpeed;
    }

    public void TakeDamage(int damage)
    {

        if (!isDead)
        {
            currentHealth -= damage;
            playerAnimator.SetTrigger("hitDamage");

            FindFirstObjectByType<UIManager>().UpdatePlayerHealth(currentHealth);

            if (currentHealth <= 0)
            {
                isDead = true;
                ZeroSpeed();
                playerAnimator.SetTrigger("Dead");

                DisablePlayer();

            }
        }





    }



    public void DisablePlayer()
    {

        if (isDead == true)
        {
    //Carregar a cena 
                SceneManager.LoadScene("GameOver");
            

            this.gameObject.SetActive(false);
        }



            

        }


    }
