[System.Serializable]
public class CraftingJob
{
    public CraftingItem craftingItem;
    public float timeRemaining;
    public bool isComplete;

    public CraftingJob(CraftingItem item)
    {
        craftingItem = item;
        timeRemaining = item.craftingTime / 1000f * 60f; // Convert to seconds
        isComplete = false;
    }
}
