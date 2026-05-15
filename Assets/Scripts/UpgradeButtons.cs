using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeButtons : MonoBehaviour
{
    public static List<Slot> UpgradeButtonsRequiedItems = new List<Slot>();
    public List<Slot> requiredItems = new List<Slot>();
    private Miner miner;
    public string sceneToLoad;

    private void Awake()
    {
        miner = GetComponent<Miner>();
    }

    bool Upgrade()
    {
        SelectedFactory.isDeleting = false;
        UpgradeButtonsRequiedItems = requiredItems;

        // Step 1: Check requirements first
        if (!Storage.Instance.HasItem(requiredItems))
        {
            AudioManager.instance.PlaySFX(audioClipType.Error);
            Debug.Log("ErrorStorageUpgradeMiner");
            return false;
        }
        else
        {
            //Step 2: Remove items
            Storage.Instance.RemoveItems(requiredItems);
            return true;
        }
        
    }

    public void UpgradeMiner()
    {

        if (UpgradeManager.Instance.minerExtractTime <= 0.1f)
        {
            Debug.Log("MinerIsMaxed");
            return;
        }


        if (!Upgrade()) return;

            //Apply upgrade
            UpgradeManager.Instance.minerExtractTime =
                Mathf.Max(0.1f, UpgradeManager.Instance.minerExtractTime - 0.25f);
            Debug.Log("Upgrade successful Miner");
    }

    public void UpgradeAssembler()
    {
        if (UpgradeManager.Instance.AssemblerCraftTime <= 0.1f)
        {
            Debug.Log("AssemblerIsMaxed");
            return;
        }

        if (!Upgrade()) return;

        //Step 3: Apply upgrade
        UpgradeManager.Instance.AssemblerCraftTime =
            Mathf.Max(0.1f, UpgradeManager.Instance.minerExtractTime - 0.25f);
        Debug.Log("Upgrade successful Assembler");
    }

    public void UpgradeBeltMove()
    {

        if (UpgradeManager.Instance.beltMoveSpeed >= 10f)
        {
            Debug.Log("BeltIsMaxed");
            return;
        }


        if (!Upgrade()) return;

        //Step 3: Apply upgrade
        UpgradeManager.Instance.beltMoveSpeed =
            Mathf.Max(0.1f, UpgradeManager.Instance.beltMoveSpeed += 0.5f);
        Debug.Log("Upgraded Belts");
    }

    public void UpgradeFurnace()
    {
        if (UpgradeManager.Instance.FurnaceSmeltSpeed <= 0.1f)
        {
            Debug.Log("FurnaceIsMaxed");
            return;
        }

        if (!Upgrade()) return;

        //apply the Upgrade
        UpgradeManager.Instance.FurnaceSmeltSpeed =
            Mathf.Max(0.1f, UpgradeManager.Instance.FurnaceSmeltSpeed - 0.25f);
        Debug.Log("UpgradedFurnace");
    }

    public void FinishGame()
    {
        if (!Upgrade()) return;

        //Finish Game
        SceneManager.LoadScene(sceneToLoad);
    }

    public void sceneLoader()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
