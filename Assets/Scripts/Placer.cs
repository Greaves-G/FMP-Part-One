using System.Collections.Generic;
using System.Threading;
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

    //public AudioClip buildSFX;

    private bool canPlace = true;
    private static HashSet<Vector3Int> takencells = new HashSet<Vector3Int>();
    private Vector3Int currentCell;

    private bool CanPlace => !takencells.Contains(currentCell);

    //rotation
    private float currentRotation;
    private const float ROTATION_STEP = 45f;

    void Update()
    {
        if (SelectedFactory.isDeleting)
        {
            HandleDeleteMode();
            return;
        }

        if(SelectedFactory.factorySelected == null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            return;
        }

        spriteRenderer.sprite = SelectedFactory.factorySelected.GetComponent<SpriteRenderer>().sprite;
        transform.localScale = SelectedFactory.factorySelected.transform.localScale;

        HandleRotation();

        SnapToGrid();
        MouseToBiome();
        updateColour();
        
        if(Input.GetMouseButton(1) && canPlace && CanPlace)
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
        float offset = SelectedFactory.factorySelected.CompareTag("Rotatable") ? 0 : 0f;
        Vector3 finalPos = new Vector3(snappedPos.x, snappedPos.y + offset);
        currentCell = tilemap.WorldToCell(mouseWorldPos);

        transform.position = finalPos;
    }

    void MouseToBiome()
    {
        if (WorldGen.instance == null) return;

        int halfSize = WorldGen.instance.mapSize / 2;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        int biomeX = cellPos.x + halfSize;
        int biomeY = cellPos.y + halfSize;

        if (biomeX < 0 || biomeY <0 ||
            biomeX >= WorldGen.instance.mapSize ||
            biomeY >= WorldGen.instance.mapSize)
        {
            canPlace = false;
            return;
        }

        Biome biome = WorldGen.instance.biomeMap[biomeX, biomeY];

        if (biome.canBePlacedOn)
        {
            canPlace = true;
            return;
        }

        canPlace = false;
    }


    void updateColour()
    {
        spriteRenderer.color = canPlace ? VailColour : InvailedColour;
    }

    void PlaceObject()
    {
        Instantiate(SelectedFactory.factorySelected, transform.position, Quaternion.Euler(0f, 0f, currentRotation)).GetComponent<GridPosition>().position = currentCell;
        takencells.Add(currentCell);
        Storage.Instance.RemoveItems(SelectedFactory.factorySelectedRequiredItems);
        AudioManager.instance.PlaySFX(audioClipType.Build);
        //SelectedFactory.factorySelected = null;
        //SelectedFactory.factorySelectedRequiredItems = null;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
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
    }*/

    void HandleRotation()
    {
        if (!SelectedFactory.factorySelected.CompareTag("Rotatable"))
        {
            currentRotation = 0f;
            transform.rotation = Quaternion.identity;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentRotation -= ROTATION_STEP;
            }
            else
                currentRotation += ROTATION_STEP;

            currentRotation %= 360f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    void HandleDeleteMode()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);

        if (Input.GetMouseButton(0))
        {
            TryDeleteAt(cellPos);
        }
    }

    void TryDeleteAt(Vector3Int cellPos)
    {
        GridPosition[] all = FindObjectsOfType<GridPosition>();

        foreach (var obj in all)
        {
            if (obj.position == cellPos)
            {
                takencells.Remove(cellPos);

                Destroy(obj.gameObject);

                return;
            }
        }
    }
}   
