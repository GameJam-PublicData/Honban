using System.Collections.Generic;
using StageSystem.Player;
using UnityEngine;

namespace StageSystem.Ink.Inks
{
    public class HealInkEffect : MonoBehaviour, IInkEffect
    {
        [SerializeField] private int healAmountPerTick = 1;
        [SerializeField] private float healInterval = 1f;

        readonly Dictionary<Rigidbody2D, float> _healTimers = new();
        private IInkEffect _inkEffectImplementation;
        public string MaterialName => _inkEffectImplementation.MaterialName;
        public float EffectUsageRate => 1f;

        public void StartInkArea(Rigidbody2D body)
        {
            if (_healTimers.ContainsKey(body)) return;
            _healTimers.Add(body, 0f);
        }

        public void UpdateInkArea(Rigidbody2D body)
        {
            if (!_healTimers.ContainsKey(body)) return;

            var playerHpManager = body.GetComponent<IPlayerHP>();
            if (playerHpManager == null) return;

            _healTimers[body] += Time.deltaTime;
            if (_healTimers[body] < healInterval) return;

            var tickCount = Mathf.FloorToInt(_healTimers[body] / healInterval);
            _healTimers[body] -= tickCount * healInterval;
            playerHpManager.Heal(healAmountPerTick * tickCount);
        }

        public void StopInkArea(Rigidbody2D body)
        {
            _healTimers.Remove(body);
        }


    }
}
