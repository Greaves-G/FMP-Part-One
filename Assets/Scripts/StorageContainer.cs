using UnityEngine;

public class StorageContainer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            Storage.Instance.AddItem(item.itemType, 1);
            Destroy(collision.gameObject);
        }
    }
}
