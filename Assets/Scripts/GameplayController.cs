using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    //public GameObject[] collectablePrefabs;
    //public GameObject[] obstaclePrefabs;
    //public GameObject[] enemyPrefabs;
    public string[] collectablePrefabTags;
    public string[] obstaclePrefabTags;
    public string[] enemyPrefabTags;
    public Vector3[] lanes;

    public float xValue = 2.5f, min_ObstacleDelay = 10f, max_ObstacleDelay = 40f;

    private float halfGroundSize;
    private ObjectPooler objectPooler;
    private PlayerController playerController;

    private Text score_Text;
    private int enemy_KillCount;

    //[SerializeField]
    //private GameObject onScreenControls;
    
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

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;

        halfGroundSize = GameObject.Find("GroundBlockMain").GetComponent<GroundBlock>().halfLength;
        
        playerController = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG).GetComponent<PlayerController>();

        StartCoroutine("GenerateObstacles");

        //score_Text = GameObject.Find("Score Bar").GetComponentInChildren<Text>();
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator GenerateObstacles()
    {
        float timer = Random.Range(min_ObstacleDelay, max_ObstacleDelay) / playerController.fwdMovementSpeed;

        yield return new WaitForSeconds(timer);

        CreateSpawnables(playerController.gameObject.transform.position.z + halfGroundSize);

        StartCoroutine("GenerateObstacles");
    }

    void CreateSpawnables(float zPos)
    {
        int r = Random.Range(0, 10);

        if (0 <= r && r < 7)
        {
            int obstacleLane = Random.Range(0, lanes.Length);

            AddObstacles(new Vector3(lanes[obstacleLane].x, 0f, zPos), Random.Range(0, obstaclePrefabTags.Length));

            int enemyLane = 0;

            if (obstacleLane == 0)
            {
                enemyLane = (Random.Range(0, 2) == 1) ? 1 : 2;
            }
            else if (obstacleLane == 1)
            {
                enemyLane = (Random.Range(0, 2) == 1) ? 0 : 2;
            }
            else if (obstacleLane == 2)
            {
                enemyLane = (Random.Range(0, 2) == 1) ? 1 : 0;
            }

            AddEnemies(new Vector3(lanes[enemyLane].x, 0f, zPos));

            int collectableLane = Random.Range(0, 2);

            AddCollectable(new Vector3(lanes[collectableLane].x, 0f, zPos));
        }
    }

    void AddObstacles(Vector3 pos, int type)
    {
        //GameObject obstacle = Instantiate(obstaclePrefabs[type], position, Quaternion.identity);

        GameObject obstacle = objectPooler.GetPooledObject(obstaclePrefabTags[type], pos, Quaternion.identity);

        bool mirror = Random.Range(0, 2) == 1;

        switch (type)
        {
            case 0:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                break;
            case 1:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                break;
            case 2:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -1 : 1, 0f);
                break;
            case 3:
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -170 : 170, 0f);
                break;
        }

        obstacle.transform.position = pos;
    }

    void AddEnemies(Vector3 pos, int type = 0)
    {
        int count = Random.Range(0, 3) + 1;

        for(int i=0; i<count; i++)
        {
            Vector3 shift = new Vector3(0f, 0f, Random.Range(1f, 10f) * i);

            //Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], pos + shift * i, Quaternion.identity);
            GameObject enemy = objectPooler.GetPooledObject(enemyPrefabTags[Random.Range(0, enemyPrefabTags.Length)], pos + shift * i, Quaternion.identity);
        }
    }
    
    void AddCollectable(Vector3 pos, int type = 0)
    {
        int count = Random.Range(0, 3) + 1;

        for(int i=0; i<count; i++)
        {
            Vector3 shift = new Vector3(0f, Random.Range(1f, 2f), Random.Range(1f, 10f) * i);

            //Instantiate(collectablePrefabs[Random.Range(0, collectablePrefabs.Length)], pos + shift * i, Quaternion.identity);
            GameObject collectable = objectPooler.GetPooledObject(collectablePrefabTags[Random.Range(0, collectablePrefabTags.Length)], pos + shift * i, Quaternion.identity);
        }
    }

    public void IncreaseScore()
    {
        enemy_KillCount++;
        score_Text.text = enemy_KillCount.ToString();
    }

    public void PauseGame()
    {
        //onScreenControls.SetActive(false);
        pause_Panel.SetActive(true);
        Time.timeScale = 0f;
    }    

    public void ResumeGame()
    {
        //onScreenControls.SetActive(true);
        pause_Panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver_Panel.SetActive(true);
        finalScore_Text.text = "Kiled: " + enemy_KillCount.ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);//respective name for each scene
    }
}
