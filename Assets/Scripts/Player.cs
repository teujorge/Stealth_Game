using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public event System.Action OnEndOfLevel;

    public float moveSpeed = 5;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8;

    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;
    float angle;

    Rigidbody rigidbody;
    bool disabled;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpottedPlayer += Disabled;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
            inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        float inputMagnitude = inputDirection.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        velocity = moveSpeed * smoothInputMagnitude * transform.forward;

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
        //transform.eulerAngles = Vector3.up * angle;
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + (Time.deltaTime * velocity));
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Disabled();
            OnEndOfLevel?.Invoke(); ;
        }
    }

    void Disabled()
    {
        disabled = true;
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disabled;
    }
}
