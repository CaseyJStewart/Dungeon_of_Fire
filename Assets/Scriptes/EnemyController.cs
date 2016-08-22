using UnityEngine;
using System.Collections;

enum State
{
    Idle,
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight
}
enum TowardsPlayer
{
    NotSet,
    LeftUp,
    LeftDown,
    RightUp,
    RightDown
}
public class EnemyController : MonoBehaviour
{
    State enemyState;
    TowardsPlayer TowardsState;
    private Vector3 pos;
    public float speed = 3f;
    public int health = 10;
    public GameObject Player;
    bool isTargettingPlayer = false;
    private Animator anim;
    public int idleHolder = 0;
    bool isWalking;
    public int damage = 10;
    [SerializeField]
    private int waitforseconds = 1;
    public bool canWalk = true;
    public bool isDoneWalking = false;
    void Start()
    {
        pos = transform.position;
        StartCoroutine(MyCoroutine());
        anim = this.GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    void Update()
    {
        Break();
        if (canWalk)
        {
            if (isTargettingPlayer)
            {
                if (TowardsState == TowardsPlayer.NotSet)
                {
                    Brain();
                }
            }
            if (enemyState != State.Idle)
            {
                EnemyMove(enemyState);
            }
        }
    }
    IEnumerator MyCoroutine()
    {
        yield return new WaitForSeconds(waitforseconds);
        int targetState = Random.Range(1, 5);
        if (targetState < 5)
        {
            enemyState = (State)targetState;
        }
        else
        {
            isTargettingPlayer = true;
        }
    }

    void Break()
    {
        if (health <= 0)
            Destroy(this.gameObject);
    }
    void EnemyMove(State movestate)
    {
        Vector3 move = Vector3.zero;
        float x = 0f;
        float y = 0f;
        isDoneWalking = false;
        //Debug.Log(movestate);
        if (transform.position == pos)
        {
            switch (movestate)
            {
                case State.MoveLeft:
                    // Left
                    move = Vector3.left;
                    x = -1f;
                    idleHolder = 4;
                    break;
                case State.MoveRight:
                    // Right
                    move = Vector3.right;
                    x = 1f;
                    idleHolder = 3;
                    break;
                case State.MoveUp:
                    // Up
                    move = Vector3.up;
                    y = 1f;
                    idleHolder = 2;
                    break;
                case State.MoveDown:
                    // Down
                    move = Vector3.down;
                    y = -1f;
                    idleHolder = 1;
                    break;
                default:
                    break;
            }
        }

        if (move != Vector3.zero)
        {
            //Debug.Log("has move");
            int EnemyLayer = ~LayerMask.GetMask("Enemy", "Player");
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.9f, 0f, move, 1.0f, EnemyLayer);
            //Debug.DrawRay(transform.position, move, Color.red);
            //Debug.Log(transform.position);
            if (hit.collider == null)
            {
                isWalking = true;
                pos += move;
                anim.SetBool("IsWalking", isWalking);
                anim.SetFloat("x", x);
                anim.SetFloat("y", y);
            }
            else
            {
                Debug.Log(hit.collider + ":" + hit.collider.bounds);
            }
        }
        // Debug.Log(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);    // Move there
        isWalking = false;
       
        
        if (transform.position == pos)
        {
            anim.SetBool("IsWalking", isWalking);
            isDoneWalking = true;
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
                    StartCoroutine(MyCoroutine());
            if (TowardsState != TowardsPlayer.NotSet)
            {
                switch (TowardsState)
                {
                    case TowardsPlayer.LeftDown:
                        if (enemyState == State.MoveLeft)
                            enemyState = State.MoveDown;
                        else
                        {
                            enemyState = State.Idle;
                            TowardsState = TowardsPlayer.NotSet;
                            isTargettingPlayer = false;
                        }
                        break;
                    case TowardsPlayer.LeftUp:
                        if (enemyState == State.MoveLeft)
                            enemyState = State.MoveUp;
                        else
                        {
                            enemyState = State.Idle;
                            TowardsState = TowardsPlayer.NotSet;
                            isTargettingPlayer = false;
                        }
                        break;
                    case TowardsPlayer.RightDown:
                        if (enemyState == State.MoveRight)
                            enemyState = State.MoveDown;
                        else
                        {
                            enemyState = State.Idle;
                            TowardsState = TowardsPlayer.NotSet;
                            isTargettingPlayer = false;
                        }
                        break;
                    case TowardsPlayer.RightUp:
                        if (enemyState == State.MoveRight)
                            enemyState = State.MoveUp;
                        else
                        {
                            enemyState = State.Idle;
                            TowardsState = TowardsPlayer.NotSet;
                            isTargettingPlayer = false;
                        }
                        break;
                }
            }
            else
            {
                enemyState = State.Idle;
            }
        }
    }
    void BrainFight()
    {

    }
    void Brain()
    {
        //Debug.Log("Brain");
        Vector3 move = Player.transform.position;

        if (pos.x > move.x && pos.y > move.y)
        {
            // left && down
            TowardsState = TowardsPlayer.LeftDown;
            enemyState = State.MoveLeft;
        }

        if (pos.x < move.x && pos.y > move.y)
        {
            // right && down
            TowardsState = TowardsPlayer.RightDown;
            enemyState = State.MoveRight;
        }

        if (pos.x > move.x && pos.y < move.y)
        {
            // left && up
            TowardsState = TowardsPlayer.LeftUp;
            enemyState = State.MoveLeft;
        }

        if (pos.x < move.x && pos.y < move.y)
        {
            // right && up
            TowardsState = TowardsPlayer.RightUp;
            enemyState = State.MoveRight;
        }
    }
}
