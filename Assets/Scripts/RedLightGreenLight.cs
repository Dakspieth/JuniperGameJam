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
    bool green = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        stw = GameObject.FindWithTag("Spinner").GetComponent<SpinToWin>();
        restart();
    
    }
    void Update()
    {
        timeLeft += Time.deltaTime;
        if(timeLeft<greenTime && green)
        {
          image.color = greenColor;  
        } else if(timeLeft > greenTime && green)
        {
            green = false;   
            timeLeft = 0;
        } else if(timeLeft < redTime && !green)
        {
            image.color = redColor;
            if(Mathf.Abs(pm.rb.linearVelocityX) > 0.1f && timeLeft > 0.75f)
            {
                StartCoroutine(stw.spinOverTime("You Didn't Stop For the Red Light"));
            }            
        } else
        {
            restart();
        }    
    }
    public void restart()
    {
        green = true;
        image.color = greenColor;
        greenTime = Random.Range(20, 30);
        timeLeft = 0;
    }
}
