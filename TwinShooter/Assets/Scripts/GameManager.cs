using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject deadPannel;
    [SerializeField] Text scoreText;
    private void Awake()
    {
        if (instance != this || instance == null)
            instance = this;
    }

    public float iScore;
    public Transform player;
    public Transform bulletParent;

    bool bGameOver;
    // Start is called before the first frame update
    void Start()
    {
        iScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bGameOver)
        {
            iScore += 52 * Time.deltaTime;
            scoreText.text = ((int)iScore).ToString();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        bGameOver = true;
        deadPannel.SetActive(true);
    }

    public void GetScore(int _score)
    {
        iScore += _score;

    }
}
