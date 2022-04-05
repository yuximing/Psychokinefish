using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuIntroManagerScript : MonoBehaviour
{
    public GameObject canvas;

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

    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime * 0.8f;
        switch ((int) timeElapsed)
        {
            case 0:
                break;
            case 1:
                logo.SetActive(true);
                break;
            case 2:
                creators.SetActive(true);
                break;
            case 3:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            default:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

}
