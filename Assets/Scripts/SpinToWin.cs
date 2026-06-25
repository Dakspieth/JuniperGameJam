using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class SpinToWin : MonoBehaviour
{
    public string[] baseRules = new string[] {};
    public string[] baseRuleDesc = new string[] {};
    List<string> ruleList = new List<string>();
    List<string> ruleDescList = new List<string>();
    public bool[] ruleActivated = new bool[] {}; // does nothing, basically just debug
    List<string> rulePool;
    List<string> descPool;

    TMP_Text ruleText;
    TMP_Text descText;

    PlayerMove pm;

    TMP_Text timerText;
    public float timeLeft;
        float baseTimeLeft=100;
    TMP_Text jumpsLeft;
    bool playing;
    bool startSpinning;
    TMP_Text listOfRulesText;
    string listOfRulesTemp;
    GameObject loseIMG;
    TMP_Text loseText;
    public GameObject redLightGreenLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ruleList = baseRules.ToList();
        ruleDescList = baseRuleDesc.ToList();

        newPool();
        pm = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();

        ruleText = GameObject.Find("ruleText").GetComponent<TMP_Text>();
        descText = GameObject.Find("descText").GetComponent<TMP_Text>();
        timerText = GameObject.Find("timerText").GetComponent<TMP_Text>();
        jumpsLeft = GameObject.Find("JumpsLeft").GetComponent<TMP_Text>();
        listOfRulesText = GameObject.Find("ListofRules").GetComponent<TMP_Text>();
        listOfRulesTemp = listOfRulesText.text;
        loseIMG = GameObject.Find("YouLoseImg");
        loseText = GameObject.Find("YouLoseText").GetComponent<TMP_Text>();
        //StartCoroutine(spinOverTime());
        timeLeft = baseTimeLeft;
        startSpinning = false;
        playing = true;
    }

    void Update()
    {
        //timer
        if(playing)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time Left: " + timeLeft.ToString("00.00") + "s";
            listOfRulesText.text = listOfRulesTemp;
            if(timeLeft < 0)
            {
                playing = false;
                StartCoroutine(spinOverTime("You Ran Out of Time"));
            }
            if(jumpsLeft.enabled)
            {
                jumpsLeft.text = "Jumps Left: " + (pm.maxJumps - pm.jumps).ToString();
            }
        }
        

    }

    void newPool()
    {
        rulePool = new List<string>(ruleList);
        descPool = new List<string>(ruleDescList);
        for(int i = 0; i < Mathf.Max(ruleList.Count-8, 0); i++)
        {
            int choice = Random.Range(0, rulePool.Count-1);
            rulePool.Remove(rulePool[choice]);
            descPool.Remove(descPool[choice]);
        }
        for(int i = 0; i < 8; i++)
        {
            GameObject.Find("text" + (i+1)).GetComponent<TextMeshProUGUI>().text = rulePool[i % ruleList.Count];
        }
    }

    void SetNewRules(int ruleChanged)
    {
        switch(ruleChanged)
        {
            case 0: // levatate
                pm.jetpack = true; 
                pm.jumpForce = 8f;
                listOfRulesTemp += "\n- Levatate";
                break;
            case 1: // limited jumps
                jumpsLeft.enabled = true;
                pm.maxJumps = 100;      
                listOfRulesTemp += "\n- Limited Jumps"; 
                break;
            case 2: // moon gravity
                pm.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.75f;
                listOfRulesTemp += "\n- Moon Gravity";
                break;
            case 3: // no running in the hall
                pm.noRunningInHall = true;
                listOfRulesTemp += "\n- No Running In The Hall";
                break;
            case 4:
                redLightGreenLight.SetActive(true);
                break;
            case 5: // you can run now
                pm.sprintControl.AddBinding("<Keyboard>/shift");
                listOfRulesTemp += "\n- You Can Run Now";
                break;
            case 6:
                break;
            case 7:
                break;
            case 8: // bad back
                pm.moveSpeed *= 0.8f;
                listOfRulesTemp += "\n- Bad Back";
                break;
            case 9: // jaundice
                SpriteRenderer sprite = pm.animator.GetComponent<SpriteRenderer>();
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b / 2);
                listOfRulesTemp += "\n- Contract Jaundice";
                break;
            case 10: // shrink
                pm.transform.localScale = new Vector2(pm.transform.localScale.x, pm.transform.localScale.y*0.67f);
                listOfRulesTemp += "\n- Shrink";
                break;
            case 11: // forgot contacts
                Camera.main.GetUniversalAdditionalCameraData().renderPostProcessing = true;
                timerText.color = Color.white;
                jumpsLeft.color = Color.white;
                listOfRulesText.color = Color.white;
                listOfRulesTemp += "\n- Forgot Your Contacts";
                break;
            case 12:
                break;
            
        }
        
    }

    public IEnumerator spinOverTime(string loseReason)
    {
        pm.enabled = false;
        pm.jumps = 0;
        pm.transform.position = new Vector2(-73, -1);
        pm.rb.linearVelocity = Vector2.zero;
        pm.animator.SetBool("running", false);
        playing = false;
        timeLeft = baseTimeLeft;
        timerText.text = "";
        jumpsLeft.text = "";
        listOfRulesTemp = listOfRulesText.text;
        listOfRulesText.text = "";
        pm.seriousSamPaper.SetActive(true);
        pm.seriousSamPaperCollected = false;

        loseText.text = "You Lost Because: " + loseReason;
        float duration = 1f;
        float timeElapsed = 0f;
        float xPos = loseIMG.transform.position.x;
        float height = 100 + 50*(loseText.text.Split("\n").Length+1);
        loseIMG.GetComponent<RectTransform>().sizeDelta = new Vector2(loseIMG.GetComponent<RectTransform>().sizeDelta.x, height);
        while(timeElapsed < duration)
        {
            xPos = Mathf.Lerp(xPos, 960, timeElapsed);
            loseIMG.transform.position = new Vector2(xPos, loseIMG.transform.position.y);
            yield return null;
            timeElapsed += Time.deltaTime;
        } 
        xPos = 960;
        yield return new WaitForSeconds(2);
        timeElapsed = 0f;
        while(timeElapsed < duration)
        {
            xPos = Mathf.Lerp(xPos, -960, timeElapsed);
            loseIMG.transform.position = new Vector2(xPos, loseIMG.transform.position.y);
            yield return null;
            timeElapsed += Time.deltaTime;
        } 
        duration = 2f;
        timeElapsed = 0f;
        float ypos = transform.position.y;
        while(timeElapsed < duration)
        {
            ypos = Mathf.Lerp(ypos, 540, 5*Time.deltaTime);
            transform.position = new Vector2(transform.position.x, ypos);
            ruleText.transform.position = new Vector2(transform.position.x, ypos-720);
            yield return null;
            timeElapsed += Time.deltaTime;
        } 



        //wait for click
        yield return new WaitUntil(() => startSpinning);
        startSpinning = false;


        float rotation = Random.Range(1250, 2690);
        float rotZ = 0f;

        duration = 3f;
        timeElapsed = 0f;
        while(timeElapsed < duration)
        {
            rotZ = Mathf.Lerp(rotZ, rotation, 0.05f*timeElapsed/duration);
            transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, rotZ);
            timeElapsed += Time.deltaTime;
            yield return null;
        } 
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);
        
        float toPositive = transform.eulerAngles.z + (transform.eulerAngles.z < 0 ? 180 : 0);
        int ruleSelected = Mathf.FloorToInt(((360-toPositive) % 360)/45) % rulePool.Count;

        
        
        ruleList.Remove(rulePool[ruleSelected % ruleList.Count]);
        ruleDescList.Remove(descPool[ruleSelected % ruleList.Count]);

        ruleText.text = "New Rule: " + rulePool[ruleSelected % ruleList.Count];
        descText.text = descPool[ruleSelected % ruleList.Count];

        yield return new WaitForSeconds(2);

        duration = Mathf.Round(3+Mathf.Max(descText.text.Length-50, 0)/20);
        timeElapsed = 0f;
        ypos = transform.position.y;
        while(timeElapsed < duration)
        {
            ypos = Mathf.Lerp(ypos, 1505, 5*Time.deltaTime);
            transform.position = new Vector2(transform.position.x, ypos);
            ruleText.transform.position = new Vector2(transform.position.x, ypos-720);
            yield return null;
            timeElapsed += Time.deltaTime;
        } 


        timeElapsed = 0f;
        ypos = transform.position.y;
        while(timeElapsed < duration)
        {
            ypos = Mathf.Lerp(ypos, 2400, 5*Time.deltaTime);
            transform.position = new Vector2(transform.position.x, ypos);
            ruleText.transform.position = new Vector2(transform.position.x, ypos-720);
            yield return null;
            timeElapsed += Time.deltaTime;
        } 
        
        transform.position = new Vector2(transform.position.x, 2400);
        ruleText.transform.position = new Vector2(transform.position.x, 1680);

        pm.enabled = true;
        playing = true;
    
        for(int i = 0; i < baseRules.Length; i++)
        {
            if(baseRules[i] == rulePool[ruleSelected])
            {
                ruleActivated[i] = true;
                SetNewRules(4);
            }
        }
        newPool();
    }

    public void startSpin()
    {
        startSpinning = true;
    }


    
}
