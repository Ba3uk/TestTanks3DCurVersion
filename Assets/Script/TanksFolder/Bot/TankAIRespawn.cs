using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankAIRespawn :MonoBehaviour  {
    [Range(1,5)]
    public int maxTankOnLevel = 3;
    [Range(1, 5)]
    public int timeToNewTank = 5;
    [SerializeField]private GameObject TankPrefab;
    private Transform[]  pointsSpawn;
    private float timeToLastSpawn = 0;

    void Start () {
        pointsSpawn = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
           pointsSpawn[i]  = transform.GetChild(i);

        }
    }


    private void Update()
    {
        if (TanksAI.CountAI <= maxTankOnLevel)
        {
            if (timeToLastSpawn <= Time.fixedTime - timeToNewTank )
            {
                timeToLastSpawn = Time.fixedTime;
                SpawnNewTanks();
            }
        }
    }

    public  void  SpawnNewTanks()
    {
        if (TanksAI.CountAI >= maxTankOnLevel)
            return;
            
            int x = Random.Range(0, pointsSpawn.Length);
            GameObject tanksAI = Instantiate(TankPrefab, pointsSpawn[x]) as GameObject;
            NetworkServer.Spawn(tanksAI);
    }

}
