using UnityEngine;

public class Attack : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hitSound;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Ao Colidir, salva na vairavel enemy, o inimigo que foi colidido
        EnemyMeleeController enemy = collision.GetComponent<EnemyMeleeController>();
        EnemyRanged enemyRanged = collision.GetComponent<EnemyRanged>(); 

        //Ao colidir, salva na variavel player, o player que foi atingido
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(damage);


        }


        // Se a colisão foi com um inimigo
        if (enemy != null)
        {
            // Inimigo recebe dano
            enemy.TakeDamage(damage);
            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }
}
