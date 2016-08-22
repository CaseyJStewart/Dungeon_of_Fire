using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    public static int health = 100;
    public int maxHealth = 100;
    public static int defence = 10;
    public  int damage = 5;
    public static float crit = .10f;
    public BoxCollider2D[] triggerbox;
    public int level = 1;
    private Animator anim;
    public float speed = 3f;
    Vector3 pos;
    int idleHolder = 0;
    public bool canWalk = true;
    bool isWalking;
    int currentXP;
    int nextLevelUp;
    public bool isDoneWalking = false;
    void Start()
    {
        pos = transform.position;
        anim = this.GetComponent<Animator>();
        foreach(BoxCollider2D box in triggerbox)
        {
            box.enabled = false;
        }
    }

    void levelUp()
    {
        nextLevelUp += 100;
        maxHealth += 10;
    }

    void GainExperience(int amount)
    {
        currentXP += amount;
        if (currentXP >= nextLevelUp)
        {
            levelUp();
        }
    }

    void FixedUpdate()
    {
        if (canWalk == true)
        {
            PlayerMove();
        }
    }
    public void takeDamage(int damage)
    {
        health -= damage;
    }

    void PlayerMove()
    {
        foreach (BoxCollider2D box in triggerbox)
        {
            box.enabled = false;
        }
        isDoneWalking = false;
        float x = 0f;
        float y = 0f;
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.A) && transform.position == pos)
        {        // Left
            move = Vector3.left;
            x = -1f;
            idleHolder = 4;
        }
        if (Input.GetKey(KeyCode.D) && transform.position == pos)
        {        // Right
            move = Vector3.right;
            x = 1f;
            idleHolder = 3;
        }
        if (Input.GetKey(KeyCode.W) && transform.position == pos)
        {        // Up
            move = Vector3.up;
            y = 1f;
            idleHolder = 1;
        }
        if (Input.GetKey(KeyCode.S) && transform.position == pos)
        {        // Down
            move = Vector3.down;
            y = -1f;
            idleHolder = 2;
        }

        if (move != Vector3.zero)
        {
            int playerLayer = ~LayerMask.GetMask("Player");
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0f, move, 1.0f, playerLayer);
            Debug.Log(transform.position);
            if (hit.collider == null)
            {
                pos += move;
                isWalking = true;
                anim.SetBool("isWalking", isWalking);
                anim.SetFloat("x", x);
                anim.SetFloat("y", y);
            }
            else
            {
                Debug.Log(hit.collider + ":" + hit.collider.bounds);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);    // Move there
        
        if (transform.position == pos)
            foreach (BoxCollider2D box in triggerbox)
            {
                box.enabled = true;
            }
        isWalking = false;
        isDoneWalking = true;
        anim.SetBool("isWalking", isWalking);
        switch (idleHolder)
        {
            case 1:
                anim.SetFloat("Direction", 1);
                break;
            case 2:
                anim.SetFloat("Direction", 2);
                break;
            case 3:
                anim.SetFloat("Direction", 3);
                break;
            case 4:
                anim.SetFloat("Direction", 4);
                break;

            default:
                break;
        }
    }
}
