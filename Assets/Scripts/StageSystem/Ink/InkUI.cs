using System;
using UnityEngine;
using UnityEngine.UI;

namespace StageSystem.Ink
{
    public class InkUI : MonoBehaviour
    {
        [SerializeField] Slider slider;
        private InkUse _inkUse;

        void Start()
        {
            _inkUse = FindObjectOfType<InkUse>();
            slider.value = 1;
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = _inkUse.ink / 100f;
        }
    }
}
