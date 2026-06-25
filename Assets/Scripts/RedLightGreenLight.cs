using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RedLightGreenLight : MonoBehaviour
{
    float greenTime;
    public Color greenColor;
    float redTime = 2.5f;
    public Color redColor;
    Image image;
    PlayerMove pm;
    SpinToWin stw;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        stw = GameObject.FindWithTag("Spinner").GetComponent<SpinToWin>();
        StartCoroutine(redGreenLight());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator redGreenLight()
    {
        image.color = greenColor;
        greenTime = Random.Range(1, 2);
        yield return new WaitForSeconds(greenTime);
        image.color = redColor;
        yield return new WaitForSeconds(0.75f);
        if(Mathf.Abs(pm.rb.linearVelocityX) > 0.1f)
        {
            print(pm.rb.linearVelocityX);
            stw.spinOverTime("You Didn't Stop For the Red Light");
        }
        yield return new WaitForSeconds(redTime-0.75f);
        StartCoroutine(redGreenLight());

    }
}
