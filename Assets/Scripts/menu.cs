using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toGame()
    {
        SceneManager.LoadScene(1);
    }
}
