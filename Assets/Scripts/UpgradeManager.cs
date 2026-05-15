using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public float minerExtractTime = 2f;
    public float AssemblerCraftTime = 2f;
    public float beltMoveSpeed = 2f;
    public float FurnaceSmeltSpeed = 2f;

    private void Awake()
    {
        Instance = this;
    }
}

