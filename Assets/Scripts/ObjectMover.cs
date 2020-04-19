using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{

    public enum DIR { NONE = 0, UP = 1, DOWN = 2, LEFT = 4, RIGHT = 8, UPLEFT = UP + LEFT, UPRIGHT = UP + RIGHT, DONWLEFT = DOWN + LEFT, DOWNRIGHT = DOWN + RIGHT };
    // missing 3, 7 on purpose. because 3 = UP + DONN & 7 = That + LEFT

    float speed = 0f;

    private static float DEFAULT_SPEED = 5f;
    Animator ani;

    public void Move(DIR d)
    {
        doMove(d);
    }

    public void setSpeed(float newspeed)
    {
        speed = newspeed;
    }

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    void doMove(DIR d)
    {
        int dirToFace = 0;

        if (speed == 0)
        {
            Debug.LogWarning("Speed for " + gameObject + " has not been set. Defaulting to: " + DEFAULT_SPEED);
            speed = DEFAULT_SPEED;
        }

        if (FindObjectOfType<DialogController>().isDialogOnScreen())
        {
            // do nothing while dialog is on screen
            return;
        }
        Vector2 newpos = new Vector2();

        // bitwise!
        if (((int)d & (int)DIR.LEFT) == (int)DIR.LEFT)
        {
            newpos = newpos + Vector2.left;
            dirToFace = 3;
        }
        if (((int)d & (int)DIR.RIGHT) == (int)DIR.RIGHT)
        {
            newpos = newpos + Vector2.right;
            dirToFace = 1;
        }
        if (((int)d & (int)DIR.UP) == (int)DIR.UP)
        {
            newpos = newpos + Vector2.up;
            dirToFace = 4;
        }
        if (((int)d & (int)DIR.DOWN) == (int)DIR.DOWN)
        {
            newpos = newpos + Vector2.down;
            dirToFace = 2;
        }

        transform.Translate((newpos * speed) * Time.deltaTime);
        if (ani != null)
        {
            ani.SetInteger("WalkDir", dirToFace);
        }
    }
}
