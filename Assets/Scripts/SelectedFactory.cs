using Unity.VisualScripting;
using UnityEngine;

public class SelectedFactory : MonoBehaviour
{
    public static GameObject factorySelected;

    public static bool isDeleting;

    public void SetSelectedFactory(GameObject factoryToSelect)
    {
        factorySelected = factoryToSelect;
        isDeleting = false;
    }

    public void EnableIsDeleting()
    {
        isDeleting = true;
        factorySelected = null;
    }
}
