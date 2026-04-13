using UnityEngine;

public class SelectedFactory : MonoBehaviour
{
    public static GameObject factorySelected;

    public void SetSelectedFactory(GameObject factoryToSelect)
    {
        factorySelected = factoryToSelect;
    }
}
