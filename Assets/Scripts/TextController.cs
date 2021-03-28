using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    private TextMeshProUGUI text;
    public float alphaFadeTime;
    public float fadeTimeDelay;
    private float fadeTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
        fadeTimer = fadeTimeDelay;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fadeTimer -= Time.deltaTime;
        if(fadeTimer < 0)
        {
            text.alpha += Time.deltaTime / alphaFadeTime;
        }
    }

}
