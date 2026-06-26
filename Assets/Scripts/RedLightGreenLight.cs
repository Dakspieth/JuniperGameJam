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
    float timeLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        stw = GameObject.FindWithTag("Spinner").GetComponent<SpinToWin>();
    
        redGreenLight();
    }
    public void redGreenLight()
    {
        // image.color = greenColor;
        // greenTime = Random.Range(15, 28);
        // print("start");
        // while(timeLeft < greenTime)
        // {
        //     timeLeft += Time.deltaTime;
        // }
        // timeLeft = 0;
        // print("done");
        // image.color = redColor;
        // while(timeLeft < redTime)
        // {
        //     if(Mathf.Abs(pm.rb.linearVelocityX) > 0.1f && timeLeft > 0.75f)
        //     {
        //         StartCoroutine(stw.spinOverTime("You Didn't Stop For the Red Light"));
        //         return;
        //     }
        //     timeLeft += Time.deltaTime;
        // }
        
        // redGreenLight();

    }
}
