using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 origin = Vector3.zero;

    private Rigidbody rb;

    private bool grounded = false;
    private bool lockGround = false;

    public LayerMask groundMask;

    public float jumpForce = 60f;

    public const float forwardSpeed = 20.0f;
    public const float sideSpeed = 35.0f;
    public const float backSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void PM_AirMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = Camera.main.transform.right;

        Vector3 wishvel = forward * vertical + right * horizontal;
        Vector3 wishdir = wishvel.normalized;
        float wishspeed = wishvel.magnitude;

        if (wishspeed > maxSpeed)
        {
            wishvel *= maxSpeed / wishspeed;
            wishspeed = maxSpeed;
        }

        if (groundTest.isGrounded)
        {
            Velocity.y = 0;
            PM_Accelerate(wishdir, wishspeed, acceleration);
            Velocity.y -= gravity * Time.deltaTime;
            Velocity.y = Mathf.Min(Velocity.y, maxSpeed);
            // PM_GroundMove();
        }
        else
        {
            PM_AirAccelerate(wishdir, wishspeed, acceleration);

            Velocity.y -= gravity * Time.deltaTime;
            Velocity.y = Mathf.Min(Velocity.y, maxSpeed);
            // PM_FlyMove();
        }
    }

    private void PM_Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float currentspeed = Vector3.Dot(Velocity, wishdir);
        float addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;
        float accelspeed = accel * Time.deltaTime * wishspeed;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        Debug.Log(accelspeed);

        Velocity += accelspeed * wishdir;
    }

    private void PM_AirAccelerate(Vector3 wishdir, float wishspd, float accel)
    {
        if (wishspd > 30)
            wishspd = 30;
        float currentspeed = Vector3.Dot(Velocity, wishdir);
        float addspeed = wishspd - currentspeed;
        if (addspeed <= 0)
            return;
        float accelspeed = accel * Time.deltaTime * wishspd;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        Velocity += accelspeed * wishdir;
    }

    private void PM_Friction()
    {
        float speed = Velocity.magnitude;
        if (speed < 1)
        {
            Velocity.x = 0;
            Velocity.z = 0;
            return;
        }

        float curFriction = friction;
        float drop = 0;
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;
        if (groundTest.isGrounded)
        {
            start[0] = end[0] = origin[0] + Velocity[0] / speed * .425f;
            start[1] = end[1] = origin[1] + Velocity[1] / speed * .425f;
            start[2] = origin[2] + mins[2];
            end[2] = start[2] - .903f;

            // if (Physics.CapsuleCast(start, end, .425f, Vector3.down, out RaycastHit hit, .478f))
            // {
            //     curFriction *= 2.0f;
            // }


            float control = (speed < stopSpeed) ? stopSpeed : speed;
            drop += control * curFriction * Time.deltaTime;
        }

        float newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        newspeed /= speed;

        Velocity *= newspeed;
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
    }

    private void Jump()
    {
        if (!grounded)
            return;

        Vector3 velocity = rb.velocity;
        velocity.y += jumpForce;

        rb.velocity = velocity;

        lockGround = true;
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.position;

        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.0f, groundMask))
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


        PM_AirMove();


        // PM_ClipVelocity(Velocity, Vector3.up, Velocity, 1.0f);



        // PM_ClipVelocity(Velocity, Vector3.up, Velocity, 1.0f);

        MouseMove();

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
}
