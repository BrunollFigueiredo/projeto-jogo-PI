using UnityEngine;

public class Movimento : MonoBehaviour
{
    public float speed = 5f;          public float rotationSpeed = 700f;

    private Rigidbody rb;      public VariableJoystick joystick; // arraste aqui o joystick da UI
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        Vector3 movement = direction.normalized * speed * Time.deltaTime;

        // Movimentando o personagem

        rb.MovePosition(transform.position + movement);

        if (direction.magnitude >= 0.1f)
        {

            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,
            rotationSpeed * Time.deltaTime);

        }
    }
    public void Pulo()
    {
        rb.AddForce(0,6f,0,ForceMode.Impulse);
    }
}

