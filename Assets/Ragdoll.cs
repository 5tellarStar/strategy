using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 random = Random.insideUnitCircle;

        random = new Vector2(random.x, random.y < 0 ? random.y * -1 : random.y);

        rb.linearVelocity = random * 3;

        rb.angularVelocity = Random.Range(0.0f, 360.0f);

        Destroy(this.gameObject, 5);
    }
}
