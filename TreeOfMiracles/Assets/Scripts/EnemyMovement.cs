using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject snowPrefab;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed = 5f;

    private List<Vector3> points = new List<Vector3>();
    private Vector3 spawnSnowPoint = Vector3.zero;
    private Vector3 target;
    private Rigidbody rb;
    private Animator anim;
    private int indexPoint = 0;
    private int countThrow = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        print($"name={gameObject.name}    dir={direction}  mag={direction.magnitude}");
        if (direction.magnitude > 0.2f)
        {
            direction.Normalize();
            rb.AddForce(direction * speed * Time.deltaTime);
            
            // Поворачиваемся в сторону следующей точки
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Плавно поворачиваемся
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = target;
            NextPoint();
        }
    }

    private void NextPoint()
    {
        indexPoint++;
        if (indexPoint < points.Count)
        {
            target = points[indexPoint];
        }
        else
        {
            anim.SetBool("IsWalk", false);
        }
    }

    public void SetPoints(List<Vector3> pathPoints)
    {
        points.Clear();
        for (int i = 0; i < pathPoints.Count; i++)  points.Add(pathPoints[i]);
        anim.SetBool("IsWalk", true);
        indexPoint = 0;
        target = points[0];
        //print($"p1={points[0]} p2={points[1]} p3={points[2]}");
    }

    public void ReserPath()
    {
        indexPoint = 0;
        target = points[0];
        anim.SetBool("IsWalk", true);
    }

    public void ThrowSnow(int n)
    {
        countThrow += n;
        if (countThrow >= 10)
        {
            countThrow = 0;
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsThrow", true);
            //Invoke("SpawnSnow", 1f);
            Invoke("EndThrow", 4f);
        }
    }

    private void EndThrow()
    {
        anim.SetBool("IsThrow", false);
        anim.SetBool("IsWalk", true);
    }

    private void SpawnSnow()
    {
        spawnSnowPoint = transform.GetChild(0).position;
        GameObject snow = Instantiate(snowPrefab, spawnSnowPoint,Quaternion.identity);
        Destroy(snow, 5f);
    }
}
