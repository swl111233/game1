using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController111 : MonoBehaviour
{
    //hazards、hazardCounts、spawnRates的数组数量要一一对应
    //随机出现的对象
    public GameObject[] hazards;
    //出现的频率，在0-1之间，越大代表生成频率越低
    public float[] spawnRates;
    public Vector3 spawnValues;
    //出现的个数
    public int[] hazardCounts;
    //生成间隔时间
    public float spawnWait;
    //开始生成时间
    public float startWait;
    //每一波的间隔时间
    public float waveWait;

    public Text scoreText;
    private int score;

    public Text gameOverText;
    private bool gameOver;

    public Text restartText;
    private bool restart;

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        int hazardCount = 0;//统计障碍物出现的个数
        int gemInterval = Random.Range(12, 16); // 宝石出现的间隔随机设置为12到16个障碍物之后

        while (true)
        {
            for (int i = 0; i < hazards.Length; i++)//遍历所有所要生成的对象类型，方便后续更多对象随机生成
            {
                int count = hazardCounts[i];
                for (int j = 0; j < count; j++) //生成指定数量
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazards[i], spawnPosition, spawnRotation);

                    hazardCount++;

                    if (hazardCount % gemInterval == 0)// 如果已生成的障碍物数量达到了宝石间隔
                    {
                        Vector3 gemSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        Quaternion gemSpawnRotation = Quaternion.identity;
                        Instantiate(hazards[1], gemSpawnPosition, gemSpawnRotation);// 在指定位置生成宝石
                        gemInterval = Random.Range(12, 16); // 每次生成宝石后重新设置宝石出现的间隔
                    }

                    float spawnRate = spawnRates[i];
                    yield return new WaitForSeconds(spawnRate);
                }
            }

            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "点击【R】重新开始";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "分数：" + score;
    }

    void Start()
    {
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
        gameOverText.text = "";
        gameOver = false;
        restartText.text = "";
        restart = false;
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                restart = false;
            }
        }
    }

    public void GameOver()
    {
        gameOverText.text = "游戏结束";
        gameOver = true;
    }
}

