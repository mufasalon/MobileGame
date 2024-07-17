using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject startPanel, blockPrefab, diamondPrefab;

    [SerializeField]
    TMP_Text scoreText, diamondText, highScoreText, gamesText;

    GameObject player;
    bool isMovingForward;
    float speed;
    bool hasGameStarted, hasGameFinished;
    int score, diamonds, highScore, games;

    float size;
    Vector3 lastPos;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        score = 0;
        speed = 15f;
        player = GameObject.Find("Player"); 

        hasGameFinished = false;
        hasGameStarted = false;
        isMovingForward = true;

        diamonds = PlayerPrefs.HasKey("Diamond") ? PlayerPrefs.GetInt("Diamond") : 0;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        games = PlayerPrefs.HasKey("Games") ? PlayerPrefs.GetInt("Games") : 0;

        scoreText.text = score.ToString();
        diamondText.text = diamonds.ToString();
        gamesText.text = "Games Played : " + games.ToString();
        highScoreText.text = "High Score : " + highScore.ToString();

        lastPos = blockPrefab.transform.position;
        size = blockPrefab.transform.localScale.x;
        for (int i = 0; i < 10; i++)
        {
            SpawnPlatform();
        }
    }


    private void Update()
    {
        if (hasGameFinished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            return;
        }

        if (!hasGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasGameStarted = true;
                startPanel.SetActive(false);
                player.GetComponent<Rigidbody>().velocity = Vector3.right * speed;
                BlockSpawn();
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.GetComponent<Rigidbody>().velocity = (isMovingForward ? Vector3.back : Vector3.right) * speed;
            isMovingForward = !isMovingForward;
        }
    }

    void BlockSpawn()
    {
        InvokeRepeating("SpawnPlatform", 0.2f, 0.2f);
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void UpdateDiamond()
    {
        diamonds++;
        PlayerPrefs.SetInt("Diamond", diamonds);
        diamondText.text = diamonds.ToString();
    }

    public void GameOver()
    {
        hasGameFinished = true;
        startPanel.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            highScoreText.text = "High Score : " + highScore.ToString();
        }

        games++;
        gamesText.text = "Games Played : " + games.ToString();
        PlayerPrefs.SetInt("Games", games);

        CancelInvoke("SpawnPlatform");
    }

    void SpawnPlatform()
    {
        int temp = Random.Range(0, 2);
        Vector3 pos = lastPos;
        if (temp == 0)
        {
            pos.x += size;
        }
        else
        {
            pos.z -= size;
        }

        Instantiate(blockPrefab, pos, Quaternion.identity);
        lastPos = pos;

        if (Random.Range(0, 4) == 0)
        {
            Instantiate(diamondPrefab, lastPos + Vector3.up * 6f, diamondPrefab.transform.rotation);
        }
    }
}