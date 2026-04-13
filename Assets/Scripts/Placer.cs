using UnityEngine;
using UnityEngine.Tilemaps;
public class Placer : MonoBehaviour
{
    //Referances
    public Tilemap tilemap;
    public SpriteRenderer spriteRenderer;

    //colours
    public Color VailColour = Color.green;
    public Color InvailedColour = Color.red;

    private bool canPlace = true;
    void Update()
    {
        if(SelectedFactory.factorySelected == null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            return;
        }

        spriteRenderer.sprite = SelectedFactory.factorySelected.GetComponent<SpriteRenderer>().sprite;

        SnapToGrid();
        updateColour();
        
        if(Input.GetMouseButtonDown(0) && canPlace)
        {
            PlaceObject();
        }
    }

    void SnapToGrid()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
        Vector3 snappedPos = tilemap.GetCellCenterWorld(cellPos);
        Vector3 finalPos = new Vector3(snappedPos.x, snappedPos.y + 0.5f);

        transform.position = finalPos;
    }

    void updateColour()
    {
        spriteRenderer.color = canPlace ? VailColour : InvailedColour;
    }

    void PlaceObject()
    {
        Instantiate(SelectedFactory.factorySelected, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Taken")
        {
            canPlace = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Taken")
        {
            canPlace = true;
        }
    }
}   
