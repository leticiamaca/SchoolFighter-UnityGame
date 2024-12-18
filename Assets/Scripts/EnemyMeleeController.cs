using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    //Variavel que verifica se o inimigo está vivo
    public bool isDead = false;

    // Variavel para controlar o lado que o inimigo está virado
    private bool facingRight;
    public bool previousDirectionRight;




    // Variavel para detectar e armazenar posicao do player
    //Variavel que guarda posicao
    private Transform target; //Target significa alvo

    //Variaveis para movimentacao do inimigo 
    private float enemySpeed = 0.5f;
    private float currentSpeed;

    private bool isWalking;

    private float horizontalForce;
    private float verticalForce;



    private float walkTimer;

    //variaveis para mecanica de ataque
    private float attackRate = 1f;
    private float nextAttack;

    // Variáveis para mecanicas de dano
    public int maxHealth;
    public int currentHealth;
    public Sprite enemyImage;

    public float staggerTime = 0.5f;
    private float damageTimer;
    public bool isTakingDamage;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o player e armazenar sua posicao
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo 
        currentSpeed = enemySpeed;

        //Inicializar a vida do inimigo
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar se o player está para a direita ou para a esquerda
        // E com isso determinar o lado que o inimigo ficará virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }


        // Se facingRight for TRUE, vamos virar o inimigo em 180 no eixo y
        // Se o player esta na direita e a posicao anterior NAO era direita(inimigo olhando para a esquerda olhando para a esquerda)
        if (facingRight && previousDirectionRight == false)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        //Se nao vamos virar o inimigo para a esquerda
        else
        {
            this.transform.Rotate(0, 0, 0);
            //Se o player NAO estava olhando para a esquerda e a posicao anterior ERA direita (inimigo olhando para a DIREITA)
        }
        if (!facingRight && previousDirectionRight == true)
        {
            this.transform.Rotate(0, -180, 0);
            //Acabando de virar a direcao, a direcao anterior fica falsa
            previousDirectionRight = false;

        }


        //Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        //Gerenciar a animacao do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }


        //Gerenciar o tempo de stagger
        if (isTakingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;
            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTakingDamage = false;
                damageTimer = 0;
                ResetSpeed();
            }
        }

        UpdateAnimator();
    }

    private void FixedUpdate()
    {

        if (!isDead)
        {
            //MOVIMENTACAO
            // Variavel para armazenar a distancia entre o inimigo e o player
            Vector3 targetDistance = target.position - this.transform.position;

            //Determina se a forca horizontal deve ser negativa ou positiva, divide ele por ele mesmo,
            //porém o outro valor sempre será positivo, entao positivo com positivo +, negativo com positivo -,
            horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x); //Poitivo sempre, usando esse método de math

            //Entre 1 e 2 segundos, será feita uma definicao de direcao vertical
            if (walkTimer >= Random.Range(1f, 2f))
            {
                verticalForce = Random.Range(-1, 2);
                //Zerar o timer de movimentacao para andar verticalmente novamente daqui a +-1 segundo
                walkTimer = 0;

            }


            //Caso esteja perto do player, parar a movimentacao
            if (Mathf.Abs(targetDistance.x) < 0.2f)
            {
                horizontalForce = 0;
            }

            // Aplica a velocidade no inimigo fazendo o movimentar
            rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);

            //ATAQUE
            // se estiver perto do Player e o timer do jogo for maior que o valor de nextAttack
            if (Mathf.Abs(targetDistance.x) < 0.2f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                //Executa animacao de ataque
                animator.SetTrigger("Attack");
                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }
        }
    }
    void UpdateAnimator()
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTakingDamage = true;
            currentHealth -= damage;

            animator.SetTrigger("HitDamage");

            //Atualiza a UI do inimigo
            FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                isDead = true;
                ZeroSpeed();
                animator.SetTrigger("Dead");
            }
        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;

    }
    void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void DisableEnemy()
    {
        this.gameObject.SetActive(false);
    }
}
