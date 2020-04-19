using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float uppickupboxoffset = 0.1f;
    public float downpickupboxoffset = 0.25f;
    public float horzizontalpickupboxoffset = 0.15f;
    public float horizontalverticalpickupboxoffset = 0.08f;
    public float arrowRadius = 0.15f;
    Animator ani;

    public GameObject arrow;
    public Transform arrowTarget;

    private ObjectMover om;

    public BoxCollider2D pickuptrigger;
    public float speed = 5f;

    private int movedir;

    private bool moving = false;

    SoundController sound;

    public AudioMixer music;

    public static bool lasttime = false;

    GeneratorController gc;

    bool walkingRight = false;

    
    private void Awake()
    {
        ani = GetComponent<Animator>();
        gc = FindObjectOfType<GeneratorController>();
        om = gameObject.GetComponent(typeof(ObjectMover)) as ObjectMover;
        om.setSpeed(speed);
        sound = FindObjectOfType<SoundController>();

        if(!Settings.getMusic())
        {
            //disable the music
            music.SetFloat("musicVol", -80f);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (FindObjectOfType<DialogController>().isDialogOnScreen())
        {
            if (Input.GetKeyDown("e"))
            {
                FindObjectOfType<DialogController>().HideDialog();
                if(lasttime)
                {
                    sound.playSoundeffect(SoundController.SE.FLARE);
                    gc.END();
                }
            }
        }
        else
        {
            // Dialog on screen, don't move
            CalculateMovement();
            CalculatePickupBox();
            CalculatePointer();
            if (Input.GetKeyDown("e"))
            {
                GetComponentInChildren<PickUpObject>().pickupTarget();
            }
        }
    }

    void CalculatePointer()
    {
        arrow.SetActive(GetComponentInChildren<PickUpObject>().isHoldingOutForAHero());
        arrow.transform.position = this.transform.position;
        // Rotate it regardless
        Vector3 vectorToTarget = arrowTarget.position - arrow.transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.rotation = Quaternion.RotateTowards(arrow.transform.rotation, q, 360);
    }

    void CalculatePickupBox()
    {
        // make sure the pickup zone is on the right side
        Vector2 boxpos = new Vector2();
        bool changepos = true;
        switch ((ObjectMover.DIR)movedir)
        {
            case ObjectMover.DIR.UP:
                boxpos = new Vector2(0, (1 * uppickupboxoffset));
                break;
            case ObjectMover.DIR.DOWN:
                boxpos = new Vector2(0, (-1 * downpickupboxoffset));
                break;
            case ObjectMover.DIR.LEFT:
                boxpos = new Vector2((-1 * horzizontalpickupboxoffset), (-1 * horizontalverticalpickupboxoffset));
                break;
            case ObjectMover.DIR.RIGHT:
                boxpos = new Vector2((1 * horzizontalpickupboxoffset), (-1 * horizontalverticalpickupboxoffset));
                break;
            case ObjectMover.DIR.UPLEFT:
                boxpos = new Vector2((-1 * horzizontalpickupboxoffset), (1 * uppickupboxoffset));
                break;
            case ObjectMover.DIR.UPRIGHT:
                boxpos = new Vector2((1 * horzizontalpickupboxoffset), (1 * uppickupboxoffset));
                break;
            case ObjectMover.DIR.DONWLEFT:
                boxpos = new Vector2((-1 * horzizontalpickupboxoffset), (-1 * downpickupboxoffset));
                break;
            case ObjectMover.DIR.DOWNRIGHT:
                boxpos = new Vector2((1 * horzizontalpickupboxoffset), (-1 * downpickupboxoffset));
                break;
            case ObjectMover.DIR.NONE:
                changepos = false;
                // leave it where it was
                break;
            default:
                changepos = false;
                Debug.LogError("Direction not implemented.... " + (ObjectMover.DIR)movedir);
                break;
        }
        if (changepos)
        {
            pickuptrigger.transform.position = gameObject.transform.position + new Vector3(boxpos.x, boxpos.y, 0);
        }
    }

    void CalculateMovement()
    {
        moving = false;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        movedir = (int)ObjectMover.DIR.NONE;

        if (h > 0)
        {
            // right?
           movedir += (int)ObjectMover.DIR.RIGHT;
            walkingRight = true;
        }
        else if (h < 0)
        {
            // left?
            movedir += (int)ObjectMover.DIR.LEFT;
            walkingRight = false;
        }

        if (v > 0)
        {
            // up?
            movedir += (int)ObjectMover.DIR.UP;
        }
        else if (v < 0)
        {
            // down?
            movedir += (int)ObjectMover.DIR.DOWN;
        }

        if (movedir != (int)ObjectMover.DIR.NONE)
        {
            moving = true;
            om.Move((ObjectMover.DIR)movedir);
        }

        Vector3 scale = transform.localScale;
        if (walkingRight)
        {
            scale.x = -1;
        }
        else
        {
            scale.x = 1;
        }
        transform.localScale = scale;

        sound.setWalkingLoop(moving);

        if (moving)
        {
            // dosomething later
        }
        else
        {
            ani.SetInteger("WalkDir", 0);
        }
    }
}
