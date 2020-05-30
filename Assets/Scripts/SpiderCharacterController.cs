using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiderCharacterController : MonoBehaviour
{

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

    private BoxCollider colide;
    public float distToGround = 0.5f;

    public Transform projectileSpawner;

    // Start is called before the first frame update
    void Start()
    {
        colide = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacter();
        UpdateCamera();
        CheckAttack();
        CheckJump();
        Shoot();
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
        float y = Input.GetAxis("Mouse X") * turnSpeed;

        //Debug.Log(dir.normalized);
        transform.Rotate(new Vector3(0, y, 0));
        Vector3 targetVelocity = dir.normalized * speed;
        _rigidbody.AddRelativeForce(targetVelocity, ForceMode.VelocityChange);
        animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }
    

    void UpdateCamera()
    {

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
                Debug.DrawLine(transform.position, hit.point, Color.white, 2, false);
                Debug.Log("Didn't miss");
                Debug.Log(transform.position);
                Debug.Log(hit.point);
            }
            else
            {
                Debug.Log("Missed moon2SUFFER");
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
                Debug.Log("Null?");
            }
            animator.SetTrigger("Web");
        }
    }

}
