using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Controle : MonoBehaviour
{
    public float speed = 3f;
    public float pulo = 500f;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        float J = Input.GetAxis("Jump");
        transform.Translate(H*Time.deltaTime*speed,0,V*Time.deltaTime*speed);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0,J*Time.deltaTime*pulo,0,ForceMode.Impulse);
        }
    }
}
