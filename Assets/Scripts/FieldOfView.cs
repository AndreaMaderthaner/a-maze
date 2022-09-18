using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer ;
    public PathCreator pathCreator;
    Animation animations;
    public float distanceToTarget;

    Material material;
    Vector3 directionToTarget;
    public float speed = 5f;
    float distance = 0;
    private void Start()
    {
        // playerRef = GameObject.FindGameObjectWithTag("Player");
        //pathCreator = (PathCreator) GetComponent("PathCreator");
/*        StartCoroutine(FOVRoutine());
*/    }

/*    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }*/

    private void Update()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(-transform.forward, directionToTarget) < angle / 2)
            {
                print(directionToTarget);
                distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;



/*        material =GetComponent<Renderer>().material;
*/      animations =GetComponent<Animation>();
        if (canSeePlayer)
        {
/*            material.color = Color.red;
*/          animations.Play("Spider_Armature|run_ani_vor");
            transform.position += new Vector3(directionToTarget.x/60, 0, directionToTarget.z/60) ;
            float angle2 = Vector3.Angle(new Vector3(directionToTarget.x,0,directionToTarget.z), -transform.forward);
            if (angle2 > 2f)
            {
                transform.Rotate(0,angle2,0);

            }
        //    animations.Play("Attack");
        }
        else
        {
           distance -= speed * Time.deltaTime;
           transform.position = pathCreator.path.GetPointAtDistance(distance);
           animations.Play("Spider_Armature|run_ani_vor");
           transform.rotation = pathCreator.path.GetRotationAtDistance(distance) ;

            // transform.rotation = Quaternion.Euler(0,diff,0);
            // transform.Rotate(0,diff,0);

        }
    }
}