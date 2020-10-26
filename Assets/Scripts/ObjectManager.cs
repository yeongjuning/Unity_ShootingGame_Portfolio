using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyBPrefab;
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;

    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;

    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletFollowerPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;

    public GameObject explosionPrefab;

    GameObject[] enemyB;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletFollower;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;

    GameObject[] explosion;

    GameObject[] targetPool;

    void Awake()
    {
        ObjectInit();
        Generate();
    }

    void ObjectInit()
    {
        enemyB = new GameObject[1];
        enemyL = new GameObject[10];
        enemyM = new GameObject[20];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[20];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[500];
        bulletEnemyB = new GameObject[100];
        bulletFollower = new GameObject[100];
        bulletBossA = new GameObject[50];
        bulletBossB = new GameObject[1000];

        explosion = new GameObject[20];
    }

    void Generate()
    {
        EnemyGenerate();
        ItemGenerate();
        BulletGenerate();
    }

    void EnemyGenerate()
    {
        PrefabGenerate(enemyB, enemyBPrefab);
        PrefabGenerate(enemyL, enemyLPrefab);
        PrefabGenerate(enemyM, enemyMPrefab);
        PrefabGenerate(enemyS, enemySPrefab);
    }

    void ItemGenerate()
    {
        PrefabGenerate(itemCoin, itemCoinPrefab);
        PrefabGenerate(itemPower, itemPowerPrefab);
        PrefabGenerate(itemBoom, itemBoomPrefab);
    }

    void BulletGenerate()
    {
        PrefabGenerate(bulletPlayerA, bulletPlayerAPrefab);
        PrefabGenerate(bulletPlayerB, bulletPlayerBPrefab);
        PrefabGenerate(bulletEnemyA, bulletEnemyAPrefab);
        PrefabGenerate(bulletEnemyB, bulletEnemyBPrefab);
        PrefabGenerate(bulletFollower, bulletFollowerPrefab);
        PrefabGenerate(bulletBossA, bulletBossAPrefab);
        PrefabGenerate(bulletBossB, bulletBossBPrefab);
        PrefabGenerate(explosion, explosionPrefab);
    }

    void PrefabGenerate(GameObject[] gameObjs, GameObject prefab)
    {
        for (int idx = 0; idx < gameObjs.Length; idx++)
        {
            gameObjs[idx] = Instantiate(prefab);
            gameObjs[idx].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        targetPool = GetPool(type);

        for (int idx = 0; idx < targetPool.Length; idx++)
        {
            if (!targetPool[idx].activeSelf)
            {
                targetPool[idx].SetActive(true);
                return targetPool[idx];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyB":
                targetPool = enemyB;
                break;
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPower":
                targetPool = itemPower;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "BulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "BulletEnemyA":
                targetPool = bulletEnemyA;
                break;
            case "BulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "BulletFollower":
                targetPool = bulletFollower;
                break;
            case "BulletBossA":
                targetPool = bulletBossA;
                break;
            case "BulletBossB":
                targetPool = bulletBossB;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }
        return targetPool;
    }
}
