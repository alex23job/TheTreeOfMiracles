using UnityEngine;

public class SnowRockControl : MonoBehaviour
{
    private int damage = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyInfo enemyInfo = collision.gameObject.GetComponent<EnemyInfo>();
            if ( enemyInfo != null)
            {
                enemyInfo.ChangeHP(damage);
            }
        }
    }
}
