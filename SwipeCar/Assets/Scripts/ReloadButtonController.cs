using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReloadButtonController : MonoBehaviour
{
    private GameObject car;
    
    // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("ReloadButton tapped!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    private void Init()
    {
        Debug.Log("Init \"ReloadButtonController\"!");
        this.car = GameObject.Find("Car");
    }
    
    private CarStatus GetCarStatus()
    {
        return car.GetComponent<CarController>().status;
    }

    private void ReloadGame()
    {
        this.car.GetComponent<CarController>().Init();
    }
}
