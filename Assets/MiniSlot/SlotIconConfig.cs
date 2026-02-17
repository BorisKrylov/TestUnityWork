using System.Collections.Generic;
using UnityEngine;

namespace MiniSlot
{
    /// <summary>
    /// Конфиг, хранящий иконки для слотов
    /// </summary>
    [CreateAssetMenu(fileName = "Slot Icon Config", menuName = "Slot/Item Config", order = 0)]
    public class SlotIconConfig : ScriptableObject
    {
        /// <summary>
        /// Иконки для слотов
        /// </summary>
        [field: SerializeField] public List<Sprite> Icons { get; private set; }
    }
}