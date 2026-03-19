using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
//MainMenuManagerで使用するためのオブジェクトをHolderとしてシリアライズフィールドで保持するクラス
public class MainMenuHolder : MonoBehaviour,IMainMenuHolder
{
    //ゲームスタートボタン関係
    [SerializeField] Button gameStartButton;
    
    //ゲームを終わるボタン関係
    [SerializeField] Button gameEndButton;
    
    //ライセンスボタン関係
    [SerializeField] Button licenseButton;
    [SerializeField] GameObject licensePanel;
    
    //音量設定関係
    [SerializeField] Button audioSettingButton;
    [SerializeField] GameObject audioSettingPanel;
    

    public Button GameStartButton => gameStartButton;
    public Button GameEndButton => gameEndButton;
    public Button LicenseButton => licenseButton;
    public GameObject LicensePanel => licensePanel;
    public Button AudioSettingButton => audioSettingButton;
    public GameObject AudioSettingPanel => audioSettingPanel;
}
}