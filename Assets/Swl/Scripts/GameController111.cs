using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController111 : MonoBehaviour
{
    //hazards��hazardCounts��spawnRates����������Ҫһһ��Ӧ
    //������ֵĶ���
    public GameObject[] hazards;
    //���ֵ�Ƶ�ʣ���0-1֮�䣬Խ���������Ƶ��Խ��
    public float[] spawnRates;
    public Vector3 spawnValues;
    //���ֵĸ���
    public int[] hazardCounts;
    //���ɼ��ʱ��
    public float spawnWait;
    //��ʼ����ʱ��
    public float startWait;
    //ÿһ���ļ��ʱ��
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

        int hazardCount = 0;//ͳ���ϰ�����ֵĸ���
        int gemInterval = Random.Range(12, 16); // ��ʯ���ֵļ���������Ϊ12��16���ϰ���֮��

        while (true)
        {
            for (int i = 0; i < hazards.Length; i++)//����������Ҫ���ɵĶ������ͣ����������������������
            {
                int count = hazardCounts[i];
                for (int j = 0; j < count; j++) //����ָ������
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazards[i], spawnPosition, spawnRotation);

                    hazardCount++;

                    if (hazardCount % gemInterval == 0)// ��������ɵ��ϰ��������ﵽ�˱�ʯ���
                    {
                        Vector3 gemSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        Quaternion gemSpawnRotation = Quaternion.identity;
                        Instantiate(hazards[1], gemSpawnPosition, gemSpawnRotation);// ��ָ��λ�����ɱ�ʯ
                        gemInterval = Random.Range(12, 16); // ÿ�����ɱ�ʯ���������ñ�ʯ���ֵļ��
                    }

                    float spawnRate = spawnRates[i];
                    yield return new WaitForSeconds(spawnRate);
                }
            }

            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "�����R�����¿�ʼ";
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
        scoreText.text = "������" + score;
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
        gameOverText.text = "��Ϸ����";
        gameOver = true;
    }
}

