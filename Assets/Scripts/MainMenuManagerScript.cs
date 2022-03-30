using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject canvas;
    public GameObject gameObjectGroup;

    GameObject logo;
    GameObject creators;

    float timeElapsed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        logo = canvas.transform.Find("Logo").gameObject;
        creators = canvas.transform.Find("Creators").gameObject;

        logo.SetActive(false);
        creators.SetActive(false);
        gameObjectGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        switch ((int) timeElapsed)
        {
            case 1:
                logo.SetActive(true);
                break;
            case 2:
                creators.SetActive(true);
                break;
            case 3:
                gameObjectGroup.SetActive(true);
                break;
            default:
                break;
        }
    }
}
