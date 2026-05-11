using TMPro;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject gameObjectButton;
    public bool currentState = false;
    public TMP_Text buttonText;
    public string textWhenOn = "Switch On";
    public string textWhenOff = "Switch Off";

    private void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<TMP_Text>();

        //updateElement();
    }

    public void ButtonClick()
    {
        currentState = !currentState;

        gameObjectButton.SetActive(!gameObjectButton.activeInHierarchy);
    }

    /*void updateElement()
    {
        gameObjectButton.SetActive(currentState);

        if (currentState)
        {
            buttonText.text = textWhenOn;
        }
        else
        {
            buttonText.text = textWhenOff;
        }
    }*/
}
