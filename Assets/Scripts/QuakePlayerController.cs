using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 origin = Vector3.zero;
    private Rigidbody rb;
    public bool grounded = false;
    private bool lockGround = false;
    private float lockGroundStart = 0.0f;
    private float lockGroundDuration = 0.1f;
    public LayerMask groundMask;
    public float forwardSpeed = 20.0f;
    public float sideSpeed = 35.0f;
    public float backSpeed = 20.0f;
    public float maxSpeed = 32.0f;
    public float stopSpeed = 20.0f;
    public float acceleration = 2.0f;
    public float airAcceleration = 0.14f;
    public float friction = 1.2f;
    public float jumpForce = 60.0f;
    public float sensitivity = 0.005f;
    public float maxStepHeight = 0.4f;

    public AudioSource[] footstepSounds;
    public GameObject footstepSoundsLocation;

    public AudioSource jumpSound;

    public GameObject gun;

    public GameObject hands;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        footstepSounds = footstepSoundsLocation.GetComponents<AudioSource>();
        foreach (AudioSource footstepSound in footstepSounds)
        {
            footstepSound.volume = 0.3f;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(FootStep());
    }

    private void MouseMove()
    {
        float sensitivity = 1.0f;
        float smoothing = 2.0f;

        Vector2 mouseDelta = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        Vector2 smoothV = Vector2.zero;

        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);

        Vector2 mouseLook = smoothV;

        Camera.main.transform.Rotate(Vector3.left * mouseLook.y);
        transform.Rotate(Vector3.up * mouseLook.x);

        // lock camera rotation
        Vector3 euler = Camera.main.transform.eulerAngles;
        // lock z rotation to 0
        euler.z = 0.0f;

        // clamp up/down rotation to 85 degrees
        if (euler.x > 85.0f && euler.x < 180.0f)
            euler.x = 85.0f;
        else if (euler.x < 275.0f && euler.x > 180.0f)
            euler.x = 275.0f;

        Camera.main.transform.eulerAngles = euler;
    }

    private void Jump()
    {
        if (!grounded)
            return;

        rb.velocity += new Vector3(0.0f, jumpForce, 0.0f);

        lockGround = true;
        grounded = false;
        lockGroundStart = Time.time;

        jumpSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;

        Ray ray = new(origin, Vector3.down);
        if (Physics.Raycast(ray, out _, 1.8f, groundMask))
        {
            if (!lockGround)
                grounded = true;
        }
        else
        {
            grounded = false;
            lockGround = false;
        }

        PM_Friction();


        float forward = 0.0f;
        float side = 0.0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            forward += forwardSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward -= backSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            side -= sideSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            side += sideSpeed;
        }

        PM_AirMove(forward, side);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Debug.Log(rb.velocity);

        // head bobbing
        float bob = Mathf.Sin(Time.time * 10.0f) * 0.20f;
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        bob *= Mathf.Min(speed / maxSpeed, 1.0f);

        Camera.main.transform.localPosition = new Vector3(0.0f, .9f + bob, 0.0f);

        // weapon swaying
        // sway in an arc
        float swayX = Mathf.Sin(Time.time * 4.0f) * 0.03f * (speed / maxSpeed);
        float swayY = -Mathf.Abs(Mathf.Cos(Time.time * 4.0f) * 0.03f) * (speed / maxSpeed);

        hands.transform.localPosition = new Vector3(swayX, swayY, 0.0f);


        MouseMove();

        if (lockGround && Time.time - lockGroundStart > lockGroundDuration)
            lockGround = false;


        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void PM_Friction()
    {
        Vector2 velocity = new(rb.velocity.x, rb.velocity.z);
        float speed = velocity.magnitude;

        if (speed < 0.1f)
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
            return;
        }

        float frictionValue = friction;

        if (grounded)
        {
            Vector2 offsetXZ = velocity / speed * 0.5f;
            Vector3 offset = new(offsetXZ.x, 0.0f, offsetXZ.y);
            Ray ray = new(transform.position + offset, Vector3.down);
            if (!Physics.Raycast(ray, out _, 1.8f, groundMask))
            {
                frictionValue *= 2.0f;
            }
        }

        float drop = 0.0f;

        if (grounded)
        {
            float control = speed < stopSpeed ? stopSpeed : speed;
            drop = control * frictionValue * Time.deltaTime;
        }

        float newSpeed = speed - drop;
        if (newSpeed < 0.0f)
            newSpeed = 0.0f;

        newSpeed /= speed;

        velocity *= newSpeed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.y);
    }

    void PM_AirMove(float fmove, float smove)
    {
        Vector3 forward, right;

        Vector3 camDir = Camera.main.transform.forward;
        right = Vector3.Cross(Vector3.up, camDir).normalized;
        forward = Vector3.Cross(right, Vector3.up).normalized;

        Vector3 wishVel = forward * fmove + right * smove;

        Vector3 wishDir = wishVel.normalized;
        float wishSpeed = wishVel.magnitude;

        if (wishSpeed > maxSpeed)
        {
            _ = maxSpeed / wishSpeed;
            wishSpeed = maxSpeed;
        }

        if (grounded)
        {
            PM_Accelerate(wishDir, wishSpeed, acceleration);
        }
        else
        {
            PM_airAccelerate(wishDir, wishSpeed, airAcceleration);
        }

        HandleSteps(wishSpeed);
    }

    void PM_Accelerate(Vector3 wishDirection, float wishSpeed, float accel)
    {
        Vector3 vel = rb.velocity;
        float currentSpeed = Vector3.Dot(vel, wishDirection);
        float addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
        {
            return;
        }

        float accelSpeed = accel * wishSpeed * Time.deltaTime;

        if (accelSpeed > addSpeed)
        {
            accelSpeed = addSpeed;
        }

        vel += accelSpeed * wishDirection;

        rb.velocity = vel;
    }

    void PM_airAccelerate(Vector3 wishDirection, float wishSpeed, float accel)
    {
        Vector3 vel = rb.velocity;
        float currentSpeed = Vector3.Dot(vel, wishDirection);
        float addSpeed = wishSpeed - currentSpeed;

        if (wishSpeed > maxSpeed)
        {
            wishSpeed = maxSpeed;
        }

        if (addSpeed <= 0)
        {
            return;
        }

        float accelSpeed = accel * Time.deltaTime * wishSpeed;

        if (accelSpeed > addSpeed)
        {
            accelSpeed = addSpeed;
        }

        vel += accelSpeed * wishDirection;

        rb.velocity = vel;
    }

    void HandleSteps(float wishSpeed)
    {
        Vector2 velocity = new(rb.velocity.x, rb.velocity.z);
        float speed = velocity.magnitude;

        if (!grounded || wishSpeed < 0.1f)
            return;

        Vector2 forward2D = velocity / speed;
        Vector3 forward = new(forward2D.x, 0.0f, forward2D.y);

        Vector3 offset = forward * 0.7f + Vector3.down * 0.99f;

        Ray ray = new(origin + offset, Vector3.up);
        Debug.DrawRay(origin + offset, Vector3.up, Color.red, 0.1f);
        Debug.DrawRay(origin, Vector3.up, Color.yellow, 0.1f);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f, groundMask))
        {
            // Debug.Log(hit.distance);
            if (hit.distance < maxStepHeight && hit.distance > 0.02f)
            {
                rb.velocity += new Vector3(0.0f, hit.distance * 10f, 0.0f);
            }
        }
    }

    void PlayRandomStepSound()
    {
        int index = Random.Range(0, footstepSounds.Length);
        footstepSounds[index].Play();
    }

    IEnumerator FootStep()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
            if (grounded && speed > 0.05f)
                PlayRandomStepSound();
        }
    }
}
