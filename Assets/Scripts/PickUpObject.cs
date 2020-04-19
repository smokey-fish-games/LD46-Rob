using UnityEngine;
using UnityEngine.UI;

public class PickUpObject : MonoBehaviour
{
    int totalWood = 0;
    int woodCount = 0;

    int totalpeople = 0;
    int peopleCount = 0;
    public Text prompt;
    bool pickupableinrange = false;
    bool holdingObject = false;
    bool canPickupNPCs = false;
    bool holdingHuman = false;

    public Image inventory;

    GameObject targetpickup;

    SoundController sound;

    public Sprite mask;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        prompt.gameObject.SetActive(pickupableinrange);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Count number of wood and dudes
        GameObject[] v = GameObject.FindGameObjectsWithTag("burn");
        totalWood = v.Length;
        v = GameObject.FindGameObjectsWithTag("npc");
        totalpeople = v.Length;
        sound = FindObjectOfType<SoundController>();

     //   Debug.Log("there are " + totalWood + " Wood");
    //    Debug.Log("there are " + totalpeople + " people");
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!holdingObject)
        {
            //   Debug.Log("I found " + other.gameObject + " with tag " + other.gameObject.tag);
            if (other.gameObject.tag == "burn")
            {
                pickupableinrange = true;
                targetpickup = other.gameObject;
            }
            else if (other.gameObject.tag == "npc")
            {
                if (canPickupNPCs)
                {
                    pickupableinrange = true;
                    targetpickup = other.gameObject;
                }
                else
                {
                    //  Debug.Log("I cannot pickup this! yet...");
                }
            }
            else
            {
                // Debug.Log("I cannot pickup this!");
            }
        }
        else
        {
            //Debug.Log("AlreadyHolding something");
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (pickupableinrange)
        {
            if (other.gameObject == targetpickup)
            {
                //Debug.Log("Lost " + targetpickup);
                pickupableinrange = false;
                targetpickup = null;
            }
        }
    }

    public bool pickupTarget()
    {
        if (!pickupableinrange || holdingObject)
        {
            return false;
        }


        pickupableinrange = false;
        inventory.sprite = targetpickup.GetComponent<SpriteRenderer>().sprite;

        if (targetpickup.tag == "burn")
        {
            woodCount++;
        }
        else if (targetpickup.tag == "npc")
        {
            peopleCount++;
            holdingHuman = true;
        }
        Destroy(targetpickup);

        holdingObject = true;

        sound.playSoundeffect(SoundController.SE.PU);
        return true;
    }

    public bool dropOffObject()
    {
        if (!holdingObject)
        {
            return false;
        }

        inventory.sprite = mask;
        holdingObject = false;

       // Debug.Log("Delivered " + woodCount + " out of " + totalWood + " wood");
       // Debug.Log("Delivered " + peopleCount + " out of " + totalpeople + " people");

        if (totalWood == woodCount)
        {
            if (!canPickupNPCs)
            {
                sound.stopMusic();
                // to only go once
                FindObjectOfType<DialogController>().ShowNextDialog();
                sound.playDarkMusic();
            }
            canPickupNPCs = true;
        }

        if (peopleCount == totalpeople)
        {
            //guess game ends here boys!
        }
        sound.playSoundeffect(SoundController.SE.PD);
        sound.playSoundeffect(SoundController.SE.FLARE);
        if(holdingHuman)
        {
            sound.playSoundeffect(SoundController.SE.SCREAM);
            holdingHuman = false;
        }
        

        return true;
    }

    public bool isHoldingOutForAHero()
    {
        return holdingObject;
    }
}
