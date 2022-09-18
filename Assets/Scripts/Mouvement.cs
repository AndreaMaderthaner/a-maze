using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{
    public float walkspeed;
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public GameObject[] spiders;
    Animation animations;
    bool breaking = false;
    bool die = false;
    // Start is called before the first frame update
    void Start()
    {
        // animations = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        // spiders = GameObject.FindGameObjectsWithTag("Spider");
        // foreach (GameObject spider in spiders)
        // {
        //     FieldOfView view = (FieldOfView) spider.GetComponent("FieldOfView");
        //     if (view.canSeePlayer && view.distanceToTarget < 5f)
        //     {
        //         breaking = true;
        //     }
        // }

        // if (die)
        // {

        // }
        // else if (breaking)
        // {
        //     // animations.Play("die");
        //     die = true;
        // }
        // else 
        // {

        float speed = walkspeed;
        string animate;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 2 * walkspeed;

            // animate = "run";
        }
        else
        {
            // animate = "walk";
        }

        if (Input.GetKey(inputFront))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            // animations.Play(animate);
        }

        if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -speed* Time.deltaTime);
            }
        if (Input.GetKey(inputLeft))
        {
            transform.Rotate(0, -0.5f, 0);
        }
        if (Input.GetKey(inputRight))
        {
            transform.Rotate(0, 0.5f, 0);
        }
    }
    
    
}
