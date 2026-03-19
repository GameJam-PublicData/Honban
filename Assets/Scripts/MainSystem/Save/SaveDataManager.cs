using System;

namespace MainSystem.Save
{
public interface ISaveDataManager
{ 
    IReadOnlySaveData Get();
    
    //SaveData変更関数を宣言していって
    void SaveStageClear(int stageIndex);
}
public class SaveDataManager : ISaveDataManager
{
    const int StageCount = 3;
    
    SaveData _saveData = new(StageCount);
    
    public IReadOnlySaveData Get()
    {
        return _saveData;
    }

    public void SaveStageClear(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= StageCount)
        {
            throw new ArgumentOutOfRangeException(nameof(stageIndex), $"stageIndex must be between 0 and {StageCount - 1}");
        }
        
        _saveData.StageData[stageIndex].IsCleared = true;
    }
}
}