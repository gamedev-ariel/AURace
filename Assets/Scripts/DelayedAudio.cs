using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
    public float delayTime = 3f; // כמה שניות לחכות (אפשר לשנות באינספקטור)

    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();

        // הפקודה הזו אומרת ליוניטי: "תפעיל את הסאונד, אבל חכה X שניות"
        audio.PlayDelayed(delayTime);
    }
}