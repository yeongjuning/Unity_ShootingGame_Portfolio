using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public int enemyScore;
    public float speed;
    public int health;
    public Sprite[] sprites;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    public float maxShotDelay;
    public float currentShotDelay;

    public GameObject bulletObj_A;
    public GameObject bulletObj_B;

    public GameObject player;
    public GameManager gameManager;
    public ObjectManager objectManager;
    public PatternManager patternManager;

    Animator anim;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyName == "B")
            anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 3000;
                Invoke("BossAppearance", 2);
                break;
            case "L":
                health = 30;
                break;
            case "M":
                health = 10;
                break;
            case "S":
                health = 3;
                break;
        }
    }

    void Update()
    {
        if (enemyName == "B")
            return;

        Fire();
        Reload();
    }

    void BossAppearance()
    {
        // BossStopPos 함수가 OnEnable 함수에 의해 두번 사용되지 않도록 조건 추가
        if (!gameObject.activeSelf)
            return;

        patternManager.BossStopPos(gameObject);
    }

    void Fire()
    {
        if (currentShotDelay < maxShotDelay)
            return;

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;

            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 4f, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4f, ForceMode2D.Impulse);
        }

        currentShotDelay = 0;
    }

    void Reload()
    {
        currentShotDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        // 아이템 다중 스폰 방지
        if (health <= 0)
            return;

        health -= damage;
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        if (health <= 0)
            Death();
    }

    public void Death()
    {
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.score += enemyScore;
        RandomItemDrop();

        CancelInvoke("ReturnSprite");
    }

    void RandomItemDrop()
    {
        // 보스의 경우에는 아이템 드랍하지 않음
        int randomItemRatio = enemyName == "B" ? 0 : Random.Range(0, 10); 
        if (randomItemRatio < 5)
        {
            Debug.Log("Not Item");
        }
        else if (randomItemRatio < 6)   // Coin : 30%
        {
            Debug.Log("Coin Drop");
            GameObject itemCoin = objectManager.MakeObj("ItemCoin");
            itemCoin.transform.position = transform.position;
            
        }
        else if (randomItemRatio < 8)   // Power : 20%
        {
            Debug.Log("Power Drop");
            GameObject itemPower = objectManager.MakeObj("ItemPower");
            itemPower.transform.position = transform.position;
        }
        else if (randomItemRatio < 10)  // Boom : 20%
        {
            Debug.Log("Boom Drop");
            GameObject itemBoom = objectManager.MakeObj("ItemBoom");
            itemBoom.transform.position = transform.position;
        }

        gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
        gameManager.ExplosionAnim(transform.position, enemyName);
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 보스의 경우 BorderBullet에 닿아도 죽지 않도록 막아놓음
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);
            collision.gameObject.SetActive(false);
        }
    }
}
