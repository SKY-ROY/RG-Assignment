using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public static WorldBuilder Instance;

    [Header("Prefabs for random spawning")]
    public GameObject[] collectablePrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject[] enemyPrefabs;
    
    [Header("Lane Vectors(x only)")]
    public Vector3[] lanes;

    [Header("Object spawn delay timers")]
    public float minObstacleDelay = 10f; 
    public float maxObstacleDelay = 40f;

    [Header("Object spawn control")]
    public bool spawnEnemies = false;
    public bool spawnObstacles = true; 
    public bool spawnCollectables = true;

    [Range(0.5f,2f)]
    public float spawnRangeMultiplier = 1f;

    private float halfGroundSize;
    private ObjectPooler objectPooler;
    private PlayerController playerController;

    private string[] collectablePrefabPoolTags;
    private string[] obstaclePrefabPoolTags;
    private string[] enemyPrefabPoolTags;

    private void Awake()
    {
        MakeInstance();

        SpecifyPoolObjectTags();
    }

    private void Start()
    {
        Initializereferences();
        StartCoroutine("GenerateObstacles");
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
        objectPooler = ObjectPooler.Instance;

        halfGroundSize = GameObject.Find("GroundBlockMain").GetComponent<GroundBlock>().halfLength;

        playerController = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG).GetComponent<PlayerController>();
    }

    void SpecifyPoolObjectTags()
    {
        GeneratePoolTagArray(collectablePrefabs, ref collectablePrefabPoolTags);
        GeneratePoolTagArray(obstaclePrefabs, ref obstaclePrefabPoolTags);
        GeneratePoolTagArray(enemyPrefabs, ref enemyPrefabPoolTags);
    }

    void GeneratePoolTagArray(GameObject[] prefabArray, ref string[] poolObjectTag)
    {
        poolObjectTag = new string[prefabArray.Length];

        for (int i = 0; i < prefabArray.Length; i++)
        {
            poolObjectTag[i] = prefabArray[i].name;
        }
    }

    IEnumerator GenerateObstacles()
    {
        float timer = Random.Range(minObstacleDelay, maxObstacleDelay) / playerController.fwdMovementSpeed;

        yield return new WaitForSeconds(timer);

        CreateSpawnables(playerController.gameObject.transform.position.z + (halfGroundSize * spawnRangeMultiplier));

        StartCoroutine("GenerateObstacles");
    }

    void CreateSpawnables(float zPos)
    {
        int r = Random.Range(0, 10);

        if (0 <= r && r < 7)
        {
            // Spawn Obstacles
            int obstacleLane = Random.Range(0, lanes.Length);

            if (spawnObstacles)
            {
                AddObstacles(new Vector3(lanes[obstacleLane].x, 0f, zPos), Random.Range(0, obstaclePrefabPoolTags.Length));
            }

            // Spawn Enemies
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

            if (spawnEnemies)
            {
                AddEnemies(new Vector3(lanes[enemyLane].x, 0f, zPos));
            }

            // Spawn Collectables
            int collectableLane = 0;

            if (obstacleLane == 0)
            {
                collectableLane = (Random.Range(0, 2) == 1) ? 1 : 2;
            }
            else if (obstacleLane == 1)
            {
                collectableLane = (Random.Range(0, 2) == 1) ? 0 : 2;
            }
            else if (obstacleLane == 2)
            {
                collectableLane = (Random.Range(0, 2) == 1) ? 1 : 0;
            }

            if (spawnCollectables)
            {
                AddCollectable(new Vector3(lanes[collectableLane].x, 0f, zPos));
            }
        }
    }

    void AddObstacles(Vector3 pos, int rotationType)
    {
        GameObject obstacle = objectPooler.GetPooledObject(obstaclePrefabPoolTags[rotationType], pos, Quaternion.identity);

        obstacle.transform.position = pos;

        /*
        bool mirror = Random.Range(0, 2) == 1;
        
        switch (rotationType)
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
                obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -180 : 180, 0f);
                break;
            default:
                obstacle.transform.rotation = Quaternion.identity;
                break;
        }
        */
    }

    void AddEnemies(Vector3 pos, int type = 0)
    {
        int count = Random.Range(0, 3) + 1;

        for (int i = 0; i < count; i++)
        {
            Vector3 shift = new Vector3(0f, 0f, Random.Range(1f, 10f) * i);

            GameObject enemy = objectPooler.GetPooledObject(enemyPrefabPoolTags[Random.Range(0, enemyPrefabPoolTags.Length)], pos + shift * i, Quaternion.identity);
        }
    }

    void AddCollectable(Vector3 pos, int type = 0)
    {
        int count = Random.Range(0, 3) + 1;

        for (int i = 0; i < count; i++)
        {
            Vector3 shift = new Vector3(0f, Random.Range(1f, 2f), Random.Range(1f, 10f) * i);

            GameObject collectable = objectPooler.GetPooledObject(collectablePrefabPoolTags[Random.Range(0, collectablePrefabPoolTags.Length)], pos + shift * (i == 0 ? 1 : i), Quaternion.identity);
        }
    }
}
