using System.Collections;
using UnityEngine;

public class footprintController : MonoBehaviour
{
    float currentA = 1.0f;

    void Awake()
    {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        while (currentA > 0)
        {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            c.a = currentA;
            yield return null;
            currentA -= Time.deltaTime;
        }
        Destroy(this.gameObject);
    }
}
