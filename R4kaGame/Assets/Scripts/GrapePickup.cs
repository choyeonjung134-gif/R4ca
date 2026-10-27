using UnityEngine;

public class GrapePickup : MonoBehaviour
{
    public int chargeAmount = 1;       // 포도 하나당 늘어나는 횟수
    public float pickupRadius = 1.5f;  // 이 거리 안으로 닿으면 먹음

    Transform player;
    AlpacaSpit spit;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            spit = p.GetComponentInChildren<AlpacaSpit>();
            if (spit == null) spit = p.GetComponentInParent<AlpacaSpit>();
        }
    }

    void Update()
    {
        if (player == null || spit == null) return;

        if (Vector3.Distance(player.position, transform.position) <= pickupRadius)
        {
            spit.AddCharge(chargeAmount);
            Destroy(gameObject);
        }
    }
}