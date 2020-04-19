using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWobbler : MonoBehaviour
{
    public float moveMin = 0.2f;
    public float moveMax = 0.5f;
    public float waitMin = 0.5f;
    public float waitMax = 1.5f;

    public float speed;
    public float maxSpeed = 3f;
    public float minSpeed = 1f;

    private ObjectMover om;
    Animator ani;

    bool moving = false;
    float moveNow = 0f;

    ObjectMover.DIR dirMove;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        om = gameObject.GetComponent(typeof(ObjectMover)) as ObjectMover;

        // Set random speed for this one object
        speed = Random.Range(minSpeed, maxSpeed);
        om.setSpeed(speed);
    }

    private void FixedUpdate()
    {
        if (!moving)
        {
            ani.SetInteger("WalkDir", 0);
            shouldMove();
            moveNow -= Time.deltaTime;
        }
        else
        {
            om.Move(dirMove);
            shouldStop();
            moveNow -= Time.deltaTime;
        }
    }

    void shouldMove()
    {
        if (moveNow <= 0)
        {
            moving = true;
            moveNow = Random.Range(moveMin, moveMax);

            do
            {
                dirMove = (ObjectMover.DIR)Random.Range(0, 11);
            } while ((int)dirMove == 3 || (int)dirMove == 7); //bad balues
                                                              //  Debug.Log("Moving for: " + moveNow + " in direction: " + dirMove);
        }
    }

    void shouldStop()
    {
        if (moveNow <= 0)
        {
            moving = false;
            moveNow = Random.Range(waitMin, waitMax);
            //   Debug.Log("Stopping for: " + moveNow);
        }
    }
}
