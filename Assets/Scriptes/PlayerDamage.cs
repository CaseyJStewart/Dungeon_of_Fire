using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour
{
    CharacterController player;
    EnemyController enemy;
    void Start()
    {
        player = FindObjectOfType<CharacterController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            enemy = col.GetComponent<EnemyController>();
            if (enemy.isDoneWalking == true)
            {
                Debug.Log("HIT");
                player.canWalk = false;
                enemy.canWalk = false;
                //MyCoroutine();
                StartCoroutine("PlayerAttack");
                StartCoroutine("EnemyAttack");
            }
        }
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("PLAYER ATTACK");

        if (Check() == 1)
        {
            PlayerWins();
            yield break;
        }
        else if (Check() == 2)
        {
            // You Lose
            Debug.Log("Player Loss");
            yield return null;
        }
        else if (Check() == -1)
        {
            enemy.TakeDamage(player.damage);
            if (Check() == 1)
            {
                PlayerWins();
                yield break;
            }
            yield return new WaitForSeconds(player.attackSpeed);
            StartCoroutine("PlayerAttack");
        }
    }
    IEnumerator EnemyAttack()
    {

        if (Check() == -1)
        {
            player.takeDamage(enemy.damage);
            Debug.Log("Delt Damage");
            yield return new WaitForSeconds(enemy.attackSpeed);
            StartCoroutine("EnemyAttack");
        }
    }

    void PlayerWins()
    {
        player.canWalk = true;
        player.GainExperience(enemy.GivingExp);
        player.GainMoney(enemy.GivingMoney);
        Debug.Log("Player Won");
    }
    int Check()
    {
        if (enemy == null)
        {
            return 1;
        }
        else if (player == null)
        {
            return 2;
        }
        else
            return -1;
    }
}