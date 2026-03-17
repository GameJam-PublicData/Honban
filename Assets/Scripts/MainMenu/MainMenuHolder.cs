using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
//MainMenuManagerで使用するためのオブジェクトをHolderとしてシリアライズフィールドで保持するクラス
public class MainMenuHolder : MonoBehaviour,IMainMenuHolder
{
    [SerializeField] Button gameEndButton;
    [SerializeField] Button licenseButton;
    [SerializeField] GameObject licensePanel;
    [SerializeField] Button audioSettingButton;
    [SerializeField] GameObject audioSettingPanel;


    public Button GameEndButton => gameEndButton;
    public Button LicenseButton => licenseButton;
    public GameObject LicensePanel => licensePanel;
    public Button AudioSettingButton => audioSettingButton;
    public GameObject AudioSettingPanel => audioSettingPanel;
}
}