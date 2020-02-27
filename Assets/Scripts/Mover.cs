using UnityEngine;

public class Mover : MonoBehaviour
{
    Rigidbody body = null;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 force =
            Input.GetAxis("Horizontal") * Vector3.right +
            Input.GetAxis("Vertical") * Vector3.forward;
        force *= 400f * Time.fixedDeltaTime;
        body.AddForce(force);
    }
}
