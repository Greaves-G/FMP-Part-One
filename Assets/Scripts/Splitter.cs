using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Splitter : Conveyor
{
    private int exitIndex = 1;

    protected override IEnumerator HandleItemTransfer(Item item)
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
        item.MarksVisited(this);

        if (transferPoints.Count > 0)
        {
            yield return MoveItemConstantSpeed(item.transform, transferPoints[0].position);
        }

        if (transferPoints.Count > 1)
        {
            Transform exitPoint = transferPoints[exitIndex];

            exitIndex++;
            if (exitIndex >= transferPoints.Count)
                exitIndex = 1;

            yield return MoveItemConstantSpeed(item.transform, exitPoint.position);
        }

        yield return new WaitForSeconds(0.05f);
        item.beingTransfered = false;
    }
}
