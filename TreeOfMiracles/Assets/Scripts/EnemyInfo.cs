using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    private int healt = 10;

    public int HP { get { return healt; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHP(int hp)
    {
        healt = hp;
    }

    public void ChangeHP(int damage)
    {
        if (healt > damage)
        {
            healt -= damage;
        }
        else
        {
            healt = 0;
        }
    }
}
