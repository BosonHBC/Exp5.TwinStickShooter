using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour
{
    public int inSceneID;
    public static GameLoopManager instance;
    public float minimumWaitTime = 2f;
    [SerializeField] Text loadingText;
    [SerializeField]Transform canvas;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene(int _id)
    {
        StartCoroutine(LoadAsynchronously(_id));
        inSceneID = _id;
    }
    private float collpaseTime;
    IEnumerator LoadAsynchronously(int _sceneID)
    {
        canvas.gameObject.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneID);
        //   SceneManager.LoadScene(_name);
        asyncOperation.allowSceneActivation = false;
        collpaseTime = 0;

        Debug.Log("Loading Scene");
        while (!asyncOperation.isDone || collpaseTime < minimumWaitTime)
        {
            collpaseTime += Time.deltaTime;

            float _waitTime = collpaseTime / minimumWaitTime;
            float _progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            _progress = Mathf.Min(_waitTime, _progress);

            string loadText = "LOADING..";
            for (int i = 0; i < (int)(_progress * 5); i++)
            {
                loadText += ".";
            }
            loadingText.text = loadText;

            if (_progress >= 1f)
            {
                Debug.Log("Loading Done!");
                asyncOperation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        canvas.gameObject.SetActive(false);
    }
}
