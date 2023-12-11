using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController111 : MonoBehaviour
{
    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public Text scoreText;
    private int score;
    public Text gameOverText;
    private bool gameOver;

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {

            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                break;
            }
        }
    }

    // Start is called before the first frame update
    void UpdateScore()
    {
        scoreText.text = "分数：" + score;
    }
    void Start()
    {
        score = 0;
        StartCoroutine(SpawnWaves());
        UpdateScore();
        gameOverText.text = "";
        gameOver = false;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    public void GameOver()
    {
        gameOverText.text = "游戏结束";
        gameOver = true;
    }
}

