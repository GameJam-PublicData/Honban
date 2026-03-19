using UnityEngine;
using UnityEngine.InputSystem;

namespace StageSystem.UI
{
public class HowToPlay : MonoBehaviour
{
    [SerializeField] GameObject howToPlayUI;

    void Update()
    {
        // Input Systemのキーボード任意キー入力を1フレーム単位で検知
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            howToPlayUI.SetActive(false);
        }
    }
}
}
