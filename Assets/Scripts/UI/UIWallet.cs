using TMPro;
using UnityEngine;
using World;

namespace UI
{
    /// <summary>
    /// Component that displays the wallet value on the screen.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIWallet : MonoBehaviour
    {
        /// <summary>
        /// Label to display the wallet value.
        /// </summary>
        private TextMeshProUGUI _label;

        private void Start()
        {
            _label = GetComponent<TextMeshProUGUI>();
            WorldManager.Instance.OnPlayerMoneyChanged += UpdateLabel;
            UpdateLabel(WorldManager.Instance.GetPlayerMoney());
        }

        /// <summary>
        /// Update the label text.
        /// </summary>
        /// <param name="amount"></param>
        private void UpdateLabel(float amount)
        {
            _label.text = $"${amount}";
        }
    }
}