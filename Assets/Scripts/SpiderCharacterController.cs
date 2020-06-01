using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiderCharacterController : MonoBehaviour
{

    public static SpiderCharacterController SharedInstance;


    private bool hasControl = true;

    public Animator animator;

    public float speed = 5;
    public float jumpSpeed = 5;
    public float turnSpeed = 5;

    public GameObject _camera;
    public Rigidbody _rigidbody;


    public Transform target;
    public float targetDistance;
    public float minDistance = 5;
    public float maxDistance = 20;
    public float zoomScale = 10;
    private float rotX;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;

    private Collider colide;
    public float distToGround = 0.1f;

    public Transform projectileSpawner;


    private void Awake()
    {
        SharedInstance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        colide = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && Time.timeScale > 0)
        {
            //UpdateCharacter();

            UpdateCamera();
            CheckAttack();
            CheckJump();
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (hasControl)
        {
            UpdateCharacter();
        }
    }

    public void SetHasControl(bool control)
    {
        this.hasControl = control;
    }

    public bool GetHasControl()
    {
        return this.hasControl;
    }


    void CheckAttack()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            animator.SetTrigger("Attack");
        }
    }

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (IsGrounded())
            {
                animator.SetTrigger("Jump");
                _rigidbody.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.VelocityChange);
            }
            else
            {
                Debug.Log("Not jumping because not grounded");
            }
            
        }
    }

    bool IsGrounded()
    {
        LayerMask mask = LayerMask.GetMask("Environment");
        return Physics.Raycast(colide.bounds.center, -Vector3.up, distToGround, mask);
    }

    void UpdateCharacter()
    {

        Vector3 dir = new Vector3(0, 0, 0);
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        

        dir = transform.TransformDirection(dir);
        //Debug.Log(dir.normalized);
        

        Ray r = new Ray(transform.position, dir.normalized);
        RaycastHit hit;
        float dist = 0.25f;


        Vector3 targetVelocity = dir.normalized * speed;

        if (Physics.Raycast(r, out hit, dist, LayerMask.GetMask("Environment")))
        {
            Debug.DrawLine(r.origin, r.origin + r.direction * dist, Color.white, 2);
            Debug.DrawLine(hit.point, hit.point + hit.normal * dist, Color.red, 2);

            float normalMagnitude = Vector3.Dot(r.direction, hit.normal);

            //dir = r.direction - hit.normal * normalMagnitude;

            targetVelocity = targetVelocity - (hit.normal * normalMagnitude * speed);

            Debug.DrawLine(r.origin, r.origin + (r.direction - hit.normal * normalMagnitude) * dist, Color.green, 2);
        }

        
        Debug.DrawLine(transform.position, transform.position + dir.normalized, Color.blue);

        _rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
        animator.SetFloat("Speed", targetVelocity.magnitude);

    }
    

    void UpdateCamera()
    {

        float y = Input.GetAxis("Mouse X") * turnSpeed;
        transform.Rotate(new Vector3(0, y, 0));

        //get zoom from scrollwheel
        targetDistance += Input.mouseScrollDelta.y * zoomScale *-1;
        targetDistance = Math.Min(maxDistance, targetDistance);
        targetDistance = Math.Max(targetDistance, minDistance);

        // get the mouse inputs
        rotX += Input.GetAxis("Mouse Y") * turnSpeed;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        // rotate the camera
        _camera.transform.eulerAngles = new Vector3(-rotX, _camera.transform.eulerAngles.y, 0);

        // move the camera position
        _camera.transform.position = target.position - (_camera.transform.forward * targetDistance);
    }



    void Shoot()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Debug.DrawLine(transform.position, hit.point, Color.white, 2, false);
                //Debug.Log("Didn't miss");
                //Debug.Log(transform.position);
                //Debug.Log(hit.point);
            }
            else
            {
               // Debug.Log("Missed moon2SUFFER");
            }

            GameObject web = WebPool.SharedInstance.GetPooledObject();
            if(web != null)
            {
                
                web.transform.position = projectileSpawner.position;
                web.SetActive(true);

                Projectile p = web.GetComponent<Projectile>();
                if (p)
                {
                    p.Fire(projectileSpawner.forward);
                }
            }
            else
            {
                Debug.LogError("The web pool is empty for some reason");
            }
            animator.SetTrigger("Web");
        }
    }
}
