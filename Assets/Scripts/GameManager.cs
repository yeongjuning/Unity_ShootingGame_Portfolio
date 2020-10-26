using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImages;
    public Image[] boomImages;
    public GameObject gameOverSet;

    public ObjectManager objectManager;
    public PatternManager patternManager;

    // # 적 출연에 관련한 변수들
    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyL", "EnemyM", "EnemyS", "EnemyB" };
        ReadSpawnFile();
    }

    void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        // 리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage0") as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;
            // 리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // 텍스트 파일 닫기
        stringReader.Close();

        // 첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }

        // UI Score Update
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "L":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "S":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyCS = enemy.GetComponent<Enemy>();

        enemyCS.player = player;
        enemyCS.gameManager = this;
        enemyCS.objectManager = objectManager;
        enemyCS.patternManager = patternManager;
        
        if (enemyPoint == 5 || enemyPoint == 6)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyCS.speed * (- 1), -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyCS.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyCS.speed * (-1));
        }

        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateLifeIcon(int life)
    {
        // UI Life Init Disable
        for (int idx = 0; idx < 3; idx++)
            lifeImages[idx].color = new Color(1, 1, 1, 0);

        // UI Life Active
        for (int idx = 0; idx < life; idx++)
            lifeImages[idx].color = new Color(1, 1, 1, 1);
    }

    public void UpdateBoomIcon(int boom)
    {
        // UI Boom Init Disable
        for (int idx = 0; idx < 3; idx++)
            boomImages[idx].color = new Color(1, 1, 1, 0);

        // UI Boom Active
        for (int idx = 0; idx < boom; idx++)
            boomImages[idx].color = new Color(1, 1, 1, 1);
    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
    }

    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 4f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void LifeDiminished()
    {
        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = true;
        playerLogic.life--;
        UpdateLifeIcon(playerLogic.life);
        playerLogic.power = 1;
        for (int idx = 0; idx < playerLogic.followers.Length; idx++)
            playerLogic.followers[idx].SetActive(false);
    }

    public void ExplosionAnim(Vector3 expPos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = expPos;
        explosionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}