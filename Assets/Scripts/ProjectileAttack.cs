using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {//Ao colidir com o player, o player toma dano 
            player.TakeDamage(damage);

            // E aqui ao colidir com o player o projétil é destruido
            Destroy(this.gameObject);
        }
        //Destruir o projétil ao colidir com os limites da fase, left e right
        if (collision.CompareTag("Wall"))
        {
            //Ao colidir com a parede o projétil é destruido
            Destroy(this.gameObject);
        }
    }
}
