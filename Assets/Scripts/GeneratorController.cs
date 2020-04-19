using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorController : MonoBehaviour
{
    public GameObject makerAnchors;
    public float markerAnchorOffsetx = 25f;
    public float markerAnchorOffsety = -1f;

    public List<Image> burnMarkers;
    public Image fuelIndicatorPrefab;
    public float indicatorxoffset = 25f;

    public float burnMaxtime = 5f;
    public int currentFuel = 3;
    float currentBurn;

    public GameObject gameovertext;

    private void Start()
    {
        burnMarkers = new List<Image>();
        currentBurn = burnMaxtime;
        updateFuelIndicators();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "player")
        {
            PickUpObject p = other.gameObject.GetComponentInChildren<PickUpObject>();
            if (p == null)
            {
                // Something went seriously wrong
                Debug.LogError("Could not get PickUpObject from " + other.gameObject);
            }
            if(p.dropOffObject())
            {
                currentFuel++;
            }
        }
    }

    private void FixedUpdate()
    {
        if (FindObjectOfType<DialogController>().isDialogOnScreen())
        {
            // do nothing while dialog is on screen
            return;
        }
        // Timer for fuel running down
        currentBurn -= Time.deltaTime;
        if(currentBurn <= 0)
        {
            // put another log on the fire JOSIE
            if(currentFuel == 0)
            {
                // uhoh
                gameovertext.SetActive(true);
                Application.Quit();
                // END
            }
            currentFuel--;
            currentBurn = burnMaxtime;
        }

        updateFuelIndicators();
    }

    private void updateFuelIndicators()
    {
        // wipe current list
        foreach (Image i in burnMarkers)
        {
            Destroy(i);
        }
        burnMarkers.Clear();

        for (int i = 0; i < currentFuel; i++)
        {

            // if this is the last one then we need to shape it
            Image bi = Instantiate(fuelIndicatorPrefab, makerAnchors.transform.position, Quaternion.identity);
            if (i == 0)
            {
                bi.transform.SetParent(makerAnchors.transform);
            }
            else
            {
                bi.transform.SetParent(burnMarkers[i - 1].transform);
            }
            burnMarkers.Add(bi);

            // calculate posistion
            Vector2 pos;
            if (i == 0)
            {
                //special case
                pos = makerAnchors.gameObject.transform.position;
                pos.x += markerAnchorOffsetx;
                pos.y += markerAnchorOffsety;
            }
            else
            {
                pos = burnMarkers[i - 1].gameObject.transform.position;
                pos.x += indicatorxoffset;
            }

            bi.transform.position = pos;
           // Debug.Log("Posistion will be " + pos);

            // Now adjust the final one to match the fuel left
            if ((i + 1) >= currentFuel)
            {
                float perc = currentBurn / burnMaxtime;
               // Debug.Log("Percentage left=" + perc);
                Color cl = bi.color;
                cl.a = perc;
                bi.color = cl;
            }
        }
    }
}
