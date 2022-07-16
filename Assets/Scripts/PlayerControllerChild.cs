using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerChild : MonoBehaviour
{
    public float speed;
    public Vector2 turnSpeed;
    private Rigidbody rb;
    private MeshRenderer mr;

    public GameObject head;

    private bool skipFirstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skipFirstFrame)
        {
            skipFirstFrame = false;
        }
        else
        {
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            transform.Rotate(Vector3.up, mouseInput.x * turnSpeed.x * Time.deltaTime);
            head.transform.Rotate(Vector3.right, -mouseInput.y * turnSpeed.y * Time.deltaTime);
        }


        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 movement = (input.x * transform.right + input.y * transform.forward);

        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
    }
}
