using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public Slider playerHealthBar;


    //foto para colocar nos players
    public Image playerImage;

    public GameObject enemyUI;
    public Slider enemyHealthBar;
    public Image enemyImage;

    //Objeto para armazenar os dados do player
    private PlayerController player;


    //Timers e controles do enemyUI
    [SerializeField] private float enemyUITime = 4f;
    private float enemyTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtem os dados do player
        player = FindAnyObjectByType<PlayerController>();

        // Define o valor máximo da barra de vida = ao máximo da vida do player
        playerHealthBar.maxValue = player.maxHealth;

        // Iniciar a barra de vida cheia / HealthBar cheia
        playerHealthBar.value = playerHealthBar.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //Iniciar o timer do inimigo
        enemyTimer += Time.deltaTime; //Iniciando um contador

        //Se o tempo limite for atingido, oculta o UI e reseta o timer
        if (enemyTimer >= enemyUITime)
        {
            //Desativar a enemyUI
            enemyUI.SetActive(false);

            //Zerar o timer
            enemyTimer = 0;
        }
    }

    //Atualizar a vida do player com base no total e os ataques que ele irá sofrer / Amount seria o total da vida dele que ele está atualmente
    public void UpdatePlayerHealth(int amount)
    {
        playerHealthBar.value = amount;
    }


    //atualizar os elementos de UI do inimigo / UI inteira do inimigo de todos os inimigos
    public void UpdateEnemyUI(int maxHealth, int currentHealth, Sprite Image)
    {
        //Atualiza os dados do inimigo de acordo com o inimigo atacado
        enemyHealthBar.maxValue = maxHealth;
        enemyHealthBar.value = currentHealth;
        enemyImage.sprite = Image;

        // Zera o timer para comecar a contar 4 segundos
        enemyTimer = 0;

        //Habilita a enemyUI, deixando-a visível
        enemyUI.SetActive(true);
    }

}
