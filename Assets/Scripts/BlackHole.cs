using UnityEngine;

public class BlackHole : MonoBehaviour
{

    public ItemSO darkMatter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;

        Item item = collision.GetComponent<Item>();
        if (item == null || item.itemType == null) return;

        Storage.Instance.AddItem(darkMatter, item.itemType.value);

        Destroy(collision.gameObject);
    }


}
