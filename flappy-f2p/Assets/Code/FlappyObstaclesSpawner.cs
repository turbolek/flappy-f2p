using UnityEngine;
using System.Collections.Generic;

public class FlappyObstaclesSpawner : MonoBehaviour {

	public GameObject obstaclePrefab;
    public float obstacleInterval = 7.5f;
    public float obstacleGapMin = 3f;
    public float obstacleGapMax = 5f;
    public float distanceToFirstObstacle = 10f;
    public float obstacleHeightTweak = 2f;
    public FlappyPlayer player;

    int spawnsCounter = 0;   
	List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        //Initialize map by spawning 5 obstacles
        for (int i=0; i<5; i++)
        {
            SpawnRandomObstacle();
        }
    }

    void Update()
    {

        float playerPosition = player.transform.position.x;
        GameObject firstObstacle = spawnedObstacles[0];
        GameObject lastObstacle = spawnedObstacles[spawnedObstacles.Count -1];

        //Spawn new obstacle if player is close enough to the last obstacle
        float distanceToLastObstacle = Mathf.Abs(playerPosition - lastObstacle.transform.position.x);
        if (distanceToLastObstacle <= 30f)
        {
            SpawnRandomObstacle();
        }

        //Delete first obstacle if player is far enough from it
        float distanceToFirstObstacle = Mathf.Abs(playerPosition - firstObstacle.transform.position.x);
        if (distanceToFirstObstacle >= 20f)
        {
            DestroyFirstObstacle();
        }
    }

	void Spawn( float x, float y, float gapHeight ) {
		GameObject spawned = GameObject.Instantiate( obstaclePrefab );
        spawned.transform.parent = transform;
		spawned.transform.position = new Vector3( x, y, 0 );
		spawnedObstacles.Add( spawned );
        //Count how many obstacles have been spawned to know where to spawn the next one
        spawnsCounter++;

        Transform bottomTransform = spawned.transform.FindChild( "Bottom" );
        Transform topTransform = spawned.transform.FindChild( "Top" );
        float bottomY = -gapHeight/2;
        float topY = +gapHeight/2;
        bottomTransform.localPosition = Vector3.up * bottomY;
        topTransform.localPosition = Vector3.up * topY;
    }

    void SpawnRandomObstacle()
    {
        float x = distanceToFirstObstacle + spawnsCounter * obstacleInterval;
        float y = Random.Range(-obstacleHeightTweak, obstacleHeightTweak);
        float gap = Random.Range(obstacleGapMin, obstacleGapMax);
        Spawn(x, y, gap);
    }

    void DestroyFirstObstacle()
    {
        GameObject obstacleToDestroy = spawnedObstacles[0];
        spawnedObstacles.RemoveAt(0);
        Debug.Log("Destroing obstacle : " + obstacleToDestroy);
        Destroy(obstacleToDestroy.gameObject);
    }
}
