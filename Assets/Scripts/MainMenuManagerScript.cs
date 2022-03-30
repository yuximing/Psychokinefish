using UnityEngine;
using System.Collections;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject canvas;
    public GameObject gameObjectGroup;

    GameObject logo;
    GameObject creators;
    GameObject instructions;

    TurretScript turretScript;

    float timeElapsed = 0.0f;

    string[] instructionTexts = 
        { 
        "Use A and D to move along the rail",
        "Right click bubble objects to take/release control of them",
        "Left click to use the bubble objects",
        "Eat the food to finish the level",
        "Good luck!"
        };
    int instructionIndex = 0;
    bool[] instructionCheck = new bool[5];
    // Start is called before the first frame update
    void Start()
    {
        logo = canvas.transform.Find("Logo").gameObject;
        creators = canvas.transform.Find("Creators").gameObject;
        instructions = canvas.transform.Find("Instructions").gameObject;

        turretScript = gameObjectGroup.transform.Find("TurretSet").gameObject.transform.Find("Turret").GetComponent<TurretScript>();

        logo.SetActive(false);
        creators.SetActive(false);
        instructions.SetActive(false);
        gameObjectGroup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime * 0.8f;
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
                instructions.SetActive(true);
                break;
            default:
                break;
        }

        if (gameObjectGroup.activeSelf)
        {
            if (Input.GetKey(KeyCode.D)) StartCoroutine(CheckCoroutine(0));
            if (turretScript.IsActive) StartCoroutine(CheckCoroutine(1));
            if (gameObjectGroup.transform.Find("Blockade") == null) StartCoroutine(CheckCoroutine(2));
            if (gameObjectGroup.transform.Find("Food") == null) instructionCheck[3] = true;

            while (instructionCheck[instructionIndex]) ++instructionIndex;

            if (instructions.activeSelf) instructions.GetComponent<UnityEngine.UI.Text>().text = instructionTexts[instructionIndex];
        }

    }

    IEnumerator CheckCoroutine(int index)
    {
        yield return new WaitForSeconds(0.2f);
        instructionCheck[index] = true;
    }
}
