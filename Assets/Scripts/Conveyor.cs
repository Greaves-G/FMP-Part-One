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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;

        Item item = collision.GetComponent<Item>();

        if (item.HasVisistedRecently(this, cooldownTimer))
        {
            return;
        }

        StartCoroutine(HandleItemTransfer(item));
    }

    private IEnumerator HandleItemTransfer(Item item)
    {
        while (item.beingTransfered)
        {
            yield return null;
        }

        item.beingTransfered = true;

        item.MarksVisited(this);

        foreach (Transform point in transferPoints)
        {
            yield return MoveItemConstantSpeed(item.transform, point.position);
        }

        item.beingTransfered = false;
    }

    private IEnumerator MoveItemConstantSpeed(Transform item, Vector3 target)
    {
        float distance = Vector3.Distance(item.position, target);
        float duration = distance / moveSpeed;
        float elapsed = 0f;

        Vector3 startPos = item.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            item.position = Vector3.Lerp(startPos, target, t);
            yield return null;
        }

        item.position = target;
    }
}
