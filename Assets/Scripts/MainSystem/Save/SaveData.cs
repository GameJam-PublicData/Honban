using System.Collections.Generic;
using System.Linq;

namespace MainSystem.Save
{
public interface IReadOnlySaveData
{
    public IReadOnlyList<IReadOnlyStageSaveData> ReadOnlyStages { get; }
}
public class SaveData : IReadOnlySaveData
{
    public readonly List<StageSaveData> StageData;
    public IReadOnlyList<IReadOnlyStageSaveData> ReadOnlyStages => StageData.Select(x => (IReadOnlyStageSaveData)x).ToList();

    public SaveData(int  stageCount)
    {
        StageData = new List<StageSaveData>();
        for (int i = 0; i < stageCount; i++)
        {
            StageData.Add(new StageSaveData());
        }
    }
}

public interface IReadOnlyStageSaveData
{
    public bool IsCleared { get; }
}
public class StageSaveData :   IReadOnlyStageSaveData
{
    public bool IsCleared { get; set; }
}

}