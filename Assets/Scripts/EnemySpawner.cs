using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Lista de tipos de inimigos / array de lista de inimigos
    public GameObject[] enemyArray;

    public int numberOfEnemies;
    private int currentEnemies;
    public float spawnTime;

    public string NextSection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Nao é necessario usar
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemies >= numberOfEnemies)
        {
            //Contar a quantidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeController>(FindObjectsSortMode.None).Length;
            if(enemies <= 0)
            {
                //Avanca a fase
                LevelManager.ChangeSection(NextSection);

                //Desabilita o spawner 
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy()
    {
        // Posicao de Spawn do inimigo
        Vector2 spawnPosition;

        //Limites da fase / limites de Y
        //Para observar os limites voce pode usar a altura do player e colocar no limite em que ele pode andar, depois voce pega os dados do Y 
        //No jogo o limite superior deu entre -0.33
        //Limite inferior deu entre -0.95
        //Essas sao as áreas andáveis de Y, entao qualquer lugar entre esses dois valores os moobs podem spawnar
        spawnPosition.y = Random.Range(-0.95f, -0.36f); // é importante os valores serem float, pois dependendo da funcao usada ele nao aceita números double

        //Posicao X máximo (a direita) do confiner da camera
        //Nao irá spawnar exatamente no confiner, será colocado +1 de distancia para dar o efeito do moob andando de longe
        // Pegar o RightBound (limite direito) da section (confiner) como base 
        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        //Define o x do spawnPoint, igual ao ponto da DIREITA do confiner
        spawnPosition.x = rightSectionBound;
        //Instancia ("Spawna") os inimigos
        // Pega um inimigo aleatório da lista de inimigos
        //Spawna na posicao SpawnPosition
        //Quaternion é uma classe utilizada para trabalhar com rotacoes
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        //Incrementa o contador de inimigos do Spawner
        currentEnemies++;

        //Se o numero de inimigos atualmente na cena for menor que o numero máximo de inimigos,
        //Invoca novamente a funcao de spawn
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            //Desativa o colisor para iniciar o Spawn apenas uma vez
            //Atencao: Desabilita o Collider mas o objeto spawner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;
            //Invoca pela primeira vez a funcao SpawnEnemy
            SpawnEnemy();
        }
    }

}
