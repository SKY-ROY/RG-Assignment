using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController Instance;

    private PlayerController playerController;

    public Text coinCount_Text;
    public Text enemyKillCount_Text;
    public Text timePassed_Text;
    public Text distanceTravelled_Text;

    private int enemyKillCount;
    private int coinCount;
    private float distanceTravelled;
    private float timePassed;

    [SerializeField]
    private Button pause_Exit_Button;
    [SerializeField]
    private Button game_Over_Exit_Button;
    [SerializeField]
    private Button pause_Button;
    [SerializeField]
    private Button resume_Button;
    [SerializeField]
    private Button restart_Button;
    [SerializeField]
    private GameObject pause_Panel;
    [SerializeField]
    private GameObject gameOver_Panel;
    [SerializeField]
    private Text finalScore_Text;

    private void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        Initializereferences();
    }

    private void Update()
    {
        IncreaseDistanceTravelled();

        IncreseTimePassed();
    }

    void MakeInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    void Initializereferences()
    {        
        playerController = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG).GetComponent<PlayerController>();

        pause_Button.onClick.AddListener(PauseGame);
        resume_Button.onClick.AddListener(ResumeGame);
        pause_Exit_Button.onClick.AddListener(ExitGame);
        restart_Button.onClick.AddListener(RestartGame);
        game_Over_Exit_Button.onClick.AddListener(ExitGame);
    }
    
    public void IncreaseKillCount()
    {
        enemyKillCount++;
        
        enemyKillCount_Text.text =  $"Kills : {enemyKillCount}";
    }

    public void IncreaseCoinCount()
    {
        coinCount++;
        
        coinCount_Text.text = $"Coins : {coinCount}";
    }

    private void IncreseTimePassed()
    {
        timePassed += Time.deltaTime;
        
        timePassed_Text.text = $"Time : {timePassed.ToString("0")}s";
    }

    private void IncreaseDistanceTravelled()
    {
        distanceTravelled = Vector3.Distance(playerController.gameObject.transform.position, Vector3.zero);

        distanceTravelled_Text.text = $"Dist. : {(int)distanceTravelled}m";
    }

    public void PauseGame()
    {
        pause_Panel.SetActive(true);
        Time.timeScale = 0f;
    }    

    public void ResumeGame()
    {
        pause_Panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver_Panel.SetActive(true);
        finalScore_Text.text = $"Final Score: {enemyKillCount + (int)timePassed + (int)distanceTravelled}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);//respective name for each scene
    }
}
