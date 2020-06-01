using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public float targetDistance = 15;
    public GameObject target;
    public Renderer modelRenderer;
    bool hasBeenSeen = false;
    public float movementSpeed = 0.4f;
    bool active = false;

    public float catchDistance = 2f;


    public void Spawn() {
        Vector3 spawnPoint = RandomNavSphere(target.transform.position, targetDistance, -1);
        transform.position = spawnPoint;
        hasBeenSeen = false;
        active = true;
    }

    public void Despawn()
    {
        Vector3 v = new Vector3(100, 1, 0);
        transform.position = v;
        hasBeenSeen = false;
        TVSceneController.SharedInstance.KilledEnemy(this);
    }


    private void Update()
    {
        if (active)
        {

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if (GeometryUtility.TestPlanesAABB(planes, modelRenderer.bounds))
        {
            //can see
            hasBeenSeen = true;
        }
        else{

                if (hasBeenSeen)
                {
                    if (Random.Range(0, 2) == 1)
                    {
                        Despawn();
                    }
                    else
                    {
                        Spawn();
                    }
                }
                else
                {

                    if(SpiderCharacterController.SharedInstance != null && SpiderCharacterController.SharedInstance.GetHasControl())
                    {
                        float step = movementSpeed * Time.deltaTime * Time.timeScale; // calculate distance to move
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
                        //float dist = Vector3.Distance(transform.position, target.transform.position);
                        //Debug.Log(dist);
                        if (Vector3.Distance(transform.position, target.transform.position) < catchDistance)
                        {
                            DropRandom.SharedInstance.EventableDrop();
                            Despawn();
                        }
                    }
                }
            
        }
        
        
        
        transform.LookAt(target.transform);



        }
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
