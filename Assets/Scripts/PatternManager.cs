using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject player;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    public Enemy enemyLogic;

    public void BossStopPos(GameObject enemyObj)
    {
        Rigidbody2D rigid = enemyObj.GetComponent<Rigidbody2D>();
        enemyLogic = enemyObj.GetComponent<Enemy>();
        rigid.velocity = Vector2.zero;
        Invoke("Think", 2f);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
        
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward() // 첫번째 패턴 : 일직선 4발 발사
    {
        if (enemyLogic.health <= 0)
            return;

        // Fire 4 Bullet Foward
        GameObject bulletR = objectManager.MakeObj("BulletBossA");
        bulletR.transform.position = enemyLogic.transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossA");
        bulletRR.transform.position = enemyLogic.transform.position + Vector3.right * 0.45f;

        GameObject bulletL = objectManager.MakeObj("BulletBossA");
        bulletL.transform.position = enemyLogic.transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossA");
        bulletLL.transform.position = enemyLogic.transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8f, ForceMode2D.Impulse);

        // Pattern Counting
        PatternCouting("FireFoward", 2f);
    }

    void FireShot() // 두번째 패턴 : 샷건 형태의 방사형 공격
    {
        if (enemyLogic.health <= 0)
            return;

        // Fire Shot
        for (int idx = 0; idx < 5; idx++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = enemyLogic.transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.transform.position - enemyLogic.transform.position;
            // 위치가 겹치지 않게 랜덤 벡터를 더하여 구현
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
        }

        // Pattern Counting
        PatternCouting("FireShot", 3.5f);
    }

    void FireArc()  // 세번째 패턴 : 부채 형태의 연속 공격
    {
        if (enemyLogic.health <= 0)
            return;

        // Fire Arc
        for (int idx = 0; idx < 5; idx++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = enemyLogic.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            float bulletRotation = Mathf.Sin(Mathf.PI * 10 * (float)curPatternCount / maxPatternCount[patternIndex]);
            Vector2 dirVec = new Vector2(bulletRotation, -1);

            rigid.AddForce(dirVec.normalized * 3f, ForceMode2D.Impulse);
        }

        // Pattern Counting
        PatternCouting("FireArc", 0.15f);
    }

    void FireAround()
    {
        if (enemyLogic.health <= 0)
            return;

        // Fire Around
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int idx = 0; idx < roundNum; idx++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossB");
            bullet.transform.position = enemyLogic.transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            // 생성되는 총알의 순번을 활용하여 방향을 결정
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * idx / roundNum)
                                        ,Mathf.Sin(Mathf.PI * 2 * idx / roundNum));

            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = (Vector3.forward * 360 * idx / roundNum) + (Vector3.forward * 90);
            bullet.transform.Rotate(rotVec);
        }

        // Pattern Counting
        PatternCouting("FireAround", 0.7f);
    }

    void PatternCouting(string FunctionName, float DelayTime)
    {
        curPatternCount++;
        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke(FunctionName, DelayTime);
        else
            Invoke("Think", 3f);
    }
}
