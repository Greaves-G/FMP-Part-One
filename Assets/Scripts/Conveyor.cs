using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class Conveyor : MonoBehaviour
{
    //transfer settings
    public List<Transform> transferPoints = new List<Transform>();
    public float moveSpeed = 2f;

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
        float distance = Vector3.Distance(item.position, target);
        float duration = distance / moveSpeed;
        float elapsed = 0f;

        Vector3 startPos = item.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (item == null) break;

            item.position = Vector3.Lerp(startPos, target, t);
            yield return null;
        }

        if (item != null)
        {
            item.position = target;
        }
        
    }
}
