using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] private Transform[] enemySpawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform santaCar;
    [SerializeField] private float ThrowDelay = 1f;

    private GameObject[] enemyArr = null;
    private float timer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = ThrowDelay;
        enemyArr = new GameObject[enemySpawnPoints.Length];
        List<Vector3> points = new List<Vector3>();
        float sleep = 0f;
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            enemyArr[i] = Instantiate(enemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);
            points.Clear();
            Vector3 p1 = enemySpawnPoints[i].position;
            p1.z -= 4f;
            points.Add(p1);
            Vector3 p2 = new Vector3((p1.x + santaCar.position.x) / 2, p1.y, p1.z - 4f);
            points.Add(p2);
            points.Add(santaCar.position);
            sleep = Random.Range(0.7f, 4.5f);
            enemyArr[i].GetComponent<EnemyMovement>().SetPoints(points, sleep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isThrow = false;
        int deltaThrow = 0;
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            timer = ThrowDelay;
            isThrow = true;
        }
        for (int i = 0; i < enemyArr.Length; i++)
        {
            EnemyMovement enemyMovement = enemyArr[i].GetComponent<EnemyMovement>();
            if (isThrow)
            {
                if (enemyMovement != null)
                {
                    deltaThrow = Random.Range(1, 4);
                    enemyMovement.ThrowSnow(deltaThrow);
                }
            }
            EnemyInfo enemyInfo = enemyArr[i].GetComponent<EnemyInfo>();
            if (enemyMovement != null)
            {
                if (enemyInfo != null)
                {
                    if (enemyInfo.HP == 0 && enemyMovement != null)
                    {
                        enemyArr[i].transform.position = enemySpawnPoints[i].position;
                        enemyMovement.ResetPath();
                        enemyInfo.SetHP(10);
                    }
                }
                enemyMovement.Move();
            }
        }
    }
}
