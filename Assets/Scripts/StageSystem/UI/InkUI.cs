using StageSystem.Ink;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace StageSystem.UI
{
    public class InkUI : MonoBehaviour
    {
        [SerializeField] Slider slider;
        IInkAmount _inkAmount;

        [Inject]
        void Construct(IInkAmount inkAmount)
        {
            _inkAmount = inkAmount;
        }
        
        void Start()
        {
            slider.value = 1;
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = _inkAmount.Ink / 100f;
        }
    }
}
