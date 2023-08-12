using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blackout_Controller : MonoBehaviour {
    public GameObject blackout;

    void Awake() {
        if (visible()) {
            StartCoroutine(Fade(false, 1));
        }
    }

    public bool visible() {
        return (blackout.GetComponent<RawImage>().color.a >= 1);
    }
    
    public IEnumerator Fade(bool fadeToBlack = true, float speed = 5)
    {
        Color fadeColor = blackout.GetComponent<RawImage>().color;
        float fadeAmount;
        if (fadeToBlack) {
            while (fadeColor.a < 1) {
                fadeAmount = fadeColor.a + (speed * Time.deltaTime);
                fadeColor.a = fadeAmount;
                blackout.GetComponent<RawImage>().color = fadeColor;
                yield return null;
            }
        }
        else {
            while (fadeColor.a > 0) {
                fadeAmount = fadeColor.a - (speed * Time.deltaTime);
                fadeColor.a = fadeAmount;
                blackout.GetComponent<RawImage>().color = fadeColor;
                yield return null;
            }
        }
    }
}