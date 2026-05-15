using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Conveyor : MonoBehaviour
{
    //transfer settings
    public List<Transform> transferPoints = new List<Transform>();
    //public float moveSpeed;

    public float cooldownTimer = 1f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;

        Item item = collision.GetComponent<Item>();

        if (item.HasVisistedRecently(this, cooldownTimer))
        {
            return;
        }

        item.MarksVisited(this);
        StartCoroutine(HandleItemTransfer(item));
    }

    protected virtual IEnumerator HandleItemTransfer(Item item)
    {
        float timeWaited = 0f;
        while (item.beingTransfered)
        {
            yield return null;
            timeWaited += Time.deltaTime;
            if (timeWaited > 0.5f)
            {
                yield break;
            }
        }

        item.beingTransfered = true;

        foreach (Transform point in transferPoints)
        {
            if (item == null) break;

            yield return MoveItemConstantSpeed(item.transform, point.position);
        }

        if (item != null)
        {
            item.beingTransfered = false;
        }
    }


    protected IEnumerator MoveItemConstantSpeed(Transform item, Vector3 target)
    {
        while (Vector3.Distance(item.position, target) > 0.01f)
        {
            if (item == null) yield break;

            item.position = Vector3.MoveTowards(
                item.position,
                target,
                UpgradeManager.Instance.beltMoveSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (item != null)
        {
            item.position = target;
        }
    }


    /*private void Awake()
    {
        moveSpeed = UpgradeManager.Instance.beltMoveSpeed;
    }*/
}
