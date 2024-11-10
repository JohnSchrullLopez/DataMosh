using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SGameManager : MonoBehaviour
{
    public static SGameManager Instance;
    private TMP_Text _timer;
    private float _currentTime = 0f;
    private GameObject _startScreen;
    private GameObject _endScreen;
    private bool _paused = true;
    private static bool _allEnemiesDead = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        
        _timer = GameObject.FindGameObjectWithTag("UICanvas").transform.GetChild(0).GetComponent<TMP_Text>();
        _currentTime = 0f;
        _startScreen = GameObject.FindGameObjectWithTag("StartScreen");
        _endScreen = GameObject.FindGameObjectWithTag("EndScreen");
        _endScreen.SetActive(false);
        _paused = true;
        _allEnemiesDead = false;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        _timer.text = string.Format("{0:F}", _currentTime);
    }

    public static void BeginLevel()
    {
        Instance._startScreen.SetActive(false);
        Instance.TogglePause();
    }

    public static void EndLevel()
    {
        Instance._endScreen.SetActive(true);
        Instance.TogglePause();
        Instance._endScreen.transform.GetChild(1).GetComponent<TMP_Text>().text = "Clear Time: " + string.Format("{0:F}", Instance._currentTime);
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void TogglePause()
    {
        _paused = !_paused;
        
        if (_paused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!_paused)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public static void SetAllEnemiesDead(bool val)
    {
        _allEnemiesDead = val;
    }

    public static bool GetAllEnemiesDead()
    {
        return _allEnemiesDead;
    }
}
