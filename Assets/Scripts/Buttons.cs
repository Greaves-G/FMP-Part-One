using System.Collections;
using TMPro;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject gameObjectButton;
    public GameObject Arrow;
    public GameObject oldPage;
    public GameObject newPage;
    public bool currentState = false;
    public TMP_Text buttonText;
    //public string textWhenOn = "Switch On";
    //public string textWhenOff = "Switch Off";

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

    public void NextPageArrow()
    {
        StartCoroutine(NextPageArrowRoutine());
    }


    public IEnumerator NextPageArrowRoutine()
    {
        currentState = !currentState;

        newPage.SetActive(!newPage.activeInHierarchy);
        yield return new WaitForSeconds(0.1f);
        oldPage.SetActive(!oldPage.activeInHierarchy);
        yield return new WaitForSeconds(0.1f);
        Arrow.SetActive(!Arrow.activeInHierarchy);  
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
