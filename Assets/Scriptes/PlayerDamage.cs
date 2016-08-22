using UnityEngine;
using System.Collections;

public class PlayerDamage : MonoBehaviour
{
    CharacterController player;

    void Start()
    {
        player = FindObjectOfType<CharacterController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            EnemyController enemy = col.GetComponent<EnemyController>();
            if (enemy.isDoneWalking == true)
            {
                enemy.TakeDamage(player.damage);
                player.canWalk = false;
                enemy.canWalk = false;
            }
        }
    }
    
}