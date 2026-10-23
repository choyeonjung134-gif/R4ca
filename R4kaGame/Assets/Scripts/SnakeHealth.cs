
using UnityEngine;

public class SnakeHealth : MonoBehaviour
{
    public int maxHits = 5;
    private int currentHits = 0;

    public void TakeHit()
    {
        if (currentHits >= maxHits)
            return;

        currentHits++;

        Debug.Log("뱀이 침에 맞음: " +
                  currentHits + " / " + maxHits);

        if (currentHits >= maxHits)
        {
            Health.killCount++;
            Destroy(gameObject);
        }
    }
}
