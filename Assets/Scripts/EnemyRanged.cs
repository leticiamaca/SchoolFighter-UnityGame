using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight;
    private bool previousDirectionRight;
    private bool isDead = false;
    private Transform target;
    private float enemySpeed = 0.3f;
    private float currentSpeed;
    //Variáveis de movimentacao
    //Se as variaveis tem o mesmo valor é possível declará-las assim:
    private float verticalForce, horizontalForce;

    private bool isWalking = false;

    private float walkTimer;

    //Mecanica de dano 
    public int maxHealth;
    public int currentHealth;

    private float staggerTime = 0.5f;
    private bool isTakingDamage = false;
    private float damageTimer;

    private float attackRate = 1f;
    private float nextAttack;


    public Sprite enemyImage;

    //Variavel que vai receber o objeto do projétil, vai armazenar o projétil
    public GameObject projectile;
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
        if (horizontalForce == 0 && verticalForce == 0) //Aqui significa que ele está parado
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

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTakingDamage = true;
            currentHealth -= damage;

            animator.SetTrigger("Hurt");

            //Atualiza a UI do inimigo
            FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                //Corrige o bug do inimigo deslizar após morto
                rb.linearVelocity = Vector2.zero;
                isDead = true;
                ZeroSpeed();
                animator.SetTrigger("Dead");
            }
        }
    }

    public void FixedUpdate()
    {
        if (!isDead)
        {
            Vector3 targetDistance = target.position - this.transform.position;
            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);
                walkTimer = 0;

                if (Mathf.Abs(targetDistance.x) < 1f)
                {
                    horizontalForce = 0;
                }
                if (Mathf.Abs(targetDistance.y) < 1f)
                {
                    verticalForce = 0;
                }
                if (!isTakingDamage)
                {
                    rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
                }

                // Lógica do ataque
                if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
                {
                    //Ataque do inimigo
                    animator.SetTrigger("Attack");
                    ZeroSpeed();
                    nextAttack = Time.time + attackRate;
                }
            }
        }
    }

    public void shoot()
    {
        // define a posicao de spawn do projétil
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        //Spawnar o projétil na posicao definida
        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        //Ativar o projétil
        shotObject.SetActive(true);

        //Colocando a velocidade / fisica do tiro
        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();
        if (facingRight)
        {
            //Aplica forca no projétil para ele se deslocar para a direita
            shotPhysics.AddForceX(80f);
        }
        else
        {
            //Aplica forca no projétil para ele se deslocar para a esquerda
            shotPhysics.AddForceX(-80f);
        }

    }


    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
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
