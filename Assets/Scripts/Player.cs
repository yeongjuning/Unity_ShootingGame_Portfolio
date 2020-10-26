using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public PowerManager powerManager;
    public ObjectManager objectManager;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public float speed;

    public int life;
    public int score;
    public int maxPower;
    public int power;
    public int maxBoom;
    public int boom;

    public float maxShotDelay;
    public float currentShotDelay;

    public GameObject bulletObj_A;
    public GameObject bulletObj_B;
    public GameObject boomEffect;
    public bool isHit;
    private bool isBoomTime;
    private bool isRespawnTime;

    public GameObject[] followers;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;

    Animator anim;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3f);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;
        if (isRespawnTime)
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        else
            spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void JoyPanel(int type)
    {
        for (int idx = 0; idx < 9; idx++)
        {
            joyControl[idx] = idx == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl = false;
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }

    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Joy Control Value
        if (joyControl[0]) { h = -1; v = 1; }   // Left Top
        if (joyControl[1]) { h = 0; v = 1; }    // Center Top
        if (joyControl[2]) { h = 1; v = 1; }    // Right Top
        if (joyControl[3]) { h = -1; v = 0; }   // Left Middle
        if (joyControl[4]) { h = 0; v = 0; }    // Center Middle
        if (joyControl[5]) { h = 1; v = 0; }    // Right Middle
        if (joyControl[6]) { h = -1; v = -1; }  // Left Bottom
        if (joyControl[7]) { h = 0; v = -1; }   // Center Bottom
        if (joyControl[8]) { h = 1; v = -1; }   // Right Bottom

        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl)
            h = 0;

        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl)
            v = 0;

        Vector3 currentPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = currentPos + nextPos;

        // Animation
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            anim.SetInteger("Input", (int)h);
    }

    void Fire()
    {
        //if (!Input.GetButton("Fire1"))
        //    return;

        if (!isButtonA)
            return;
    
        if (currentShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                powerManager.PlayerPowerOne(bulletObj_A, transform.position);
                break;
            case 2:
                powerManager.PlayerPowerTwo(bulletObj_A, transform.position);
                break;
            default:
                powerManager.PlayerPowerThree(bulletObj_A, bulletObj_B, transform.position);
                break;
        }

        currentShotDelay = 0;
    }

    void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        //if (!Input.GetButton("Fire2"))
        //    return;

        if (!isButtonB)
            return;
    
        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);

        OnBoomEffect();
        HitBoomEnemy();
        RemoveEnemyBullets();
    }

    void BorderTagCase(Collider2D collision, bool isTouchBorder)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = isTouchBorder;
                    break;
                case "Bottom":
                    isTouchBottom = isTouchBorder;
                    break;
                case "Right":
                    isTouchRight = isTouchBorder;
                    break;
                case "Left":
                    isTouchLeft = isTouchBorder;
                    break;
            }
        }
    }

    void EnemyTagCase(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
                return;

            if (isHit)
                return;

            gameManager.LifeDiminished();
            gameManager.ExplosionAnim(transform.position, "P");

            if (life == 0)
                gameManager.GameOver();
            else
                gameManager.RespawnPlayer();

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
    }

    void ItemTagCase(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if (power == 4)
            followers[0].SetActive(true);
        else if (power == 5)
            followers[1].SetActive(true);
        else if (power == 6)
            followers[2].SetActive(true);
    }

    void OnBoomEffect()
    {
        // Effect Visible
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f);
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void HitBoomEnemy()
    {
        FindBoomHittedEnemy(objectManager.GetPool("EnemyB"));
        FindBoomHittedEnemy(objectManager.GetPool("EnemyL"));
        FindBoomHittedEnemy(objectManager.GetPool("EnemyM"));
        FindBoomHittedEnemy(objectManager.GetPool("EnemyS"));
    }

    void FindBoomHittedEnemy(GameObject[] hitEnemies)
    {
        for (int idx = 0; idx < hitEnemies.Length; idx++)
        {
            if (hitEnemies[idx].activeSelf)
            {
                Enemy enemyLogic = hitEnemies[idx].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }
    }

    void RemoveEnemyBullets()
    {
        SetEnemyBulletActive(objectManager.GetPool("BulletBossA"));
        SetEnemyBulletActive(objectManager.GetPool("BulletBossB"));
        SetEnemyBulletActive(objectManager.GetPool("BulletEnemyA"));
        SetEnemyBulletActive(objectManager.GetPool("BulletEnemyB"));
    }
    
    void SetEnemyBulletActive(GameObject[] enemyBullets)
    {
        for (int idx = 0; idx < enemyBullets.Length; idx++)
        {
            if (enemyBullets[idx].activeSelf)
                enemyBullets[idx].SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BorderTagCase(collision, true);
        EnemyTagCase(collision);
        ItemTagCase(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        BorderTagCase(collision, false);
    }
}