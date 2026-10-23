
using UnityEngine;

public class CrowHealth : MonoBehaviour
{
    public int maxHits = 3;

    private int currentHits = 0;


    public void TakeHit()
    {
        if (currentHits >= maxHits)
            return;

        currentHits++;

        Debug.Log("까마귀가 침에 맞음: " +
                  currentHits + " / " + maxHits);

        if (currentHits >= maxHits)
        {
            Health.killCount++;
            Destroy(gameObject);
        }
    }
}

