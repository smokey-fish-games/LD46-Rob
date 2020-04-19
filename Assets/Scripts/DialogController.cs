using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public GameObject dialogBox;
    public Text textBox;
    List<string> order = new List<string>();
    int currentpos = 0;
    bool dialogShowing = false;

    string Starttext = "This fire needs fuel! Go get some wood. 'e' to close dialog boxes";
    string humantext = "We've run out of wood and coal? But we need more fuel...";
    string pickedup = "There's no more fuel. I have no choice but to climb in. The fire must go on.";

    void Start()
    {
        order.Add(Starttext);
        order.Add(humantext);
        order.Add(pickedup);
    }

    public void ShowNextDialog()
    {
        if(currentpos >= order.Count)
        {
            Debug.Log("No more dialogs to show!");
            return;
        }

        textBox.text = order[currentpos];
        currentpos++;
        dialogBox.SetActive(true);
        dialogShowing = true;
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
        dialogShowing = false;
    }

    public bool isDialogOnScreen()
    {
        return dialogShowing;
    }
}
