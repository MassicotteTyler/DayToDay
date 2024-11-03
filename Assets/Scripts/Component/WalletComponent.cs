using System;
using UnityEngine;
using UnityEngine.Events;

namespace Component
{
    /// <summary>
    /// Component that can be attached to a GameObject to give it a wallet.
    /// </summary>
    public class WalletComponent : MonoBehaviour
    {
        /// <summary>
        /// Event that is triggered when the balance of the wallet changes.
        /// </summary>
        public UnityEvent OnBalanceChanged;

        /// <summary>
        /// Event that is triggered when money is withdrawn from the wallet.
        /// </summary>
        public UnityEvent<float> OnWithdrawal;
        
        /// <summary>
        /// Event that is triggered when money is deposited into the wallet.
        /// </summary>
        public UnityEvent<float> OnDeposit;
        
        /// <summary>
        /// The current balance of the wallet.
        /// </summary>
        private float _balance = 0;
        
        /// <summary>
        /// Public accessor for the balance of the wallet.
        /// </summary>
        public float Balance
        {
            get => _balance;
            // Private set that triggers event
            private set
            {
                _balance = value;
                OnBalanceChanged?.Invoke();
            }
        }
        
        /// <summary>
        /// Deposit money into the wallet.
        /// </summary>
        /// <param name="amount">Amount to deposit</param>
        public void Deposit(float amount)
        {
            Balance += amount;
            OnDeposit?.Invoke(amount);
        }
        
        /// <summary>
        /// Withdraw money from the wallet.
        /// </summary>
        /// <param name="amount">Amount to withdraw</param>
        public void Withdraw(float amount)
        {
            if (!CanAfford(amount)) return;
            
            Balance = Math.Clamp(Balance - amount, 0, _balance);
            OnWithdrawal?.Invoke(amount);
        }
        
        /// <summary>
        /// Check if the wallet can afford a certain amount.
        /// </summary>
        /// <param name="amount">The amount to check</param>
        /// <returns>If the wallet balance is higher than the requested amount</returns>
        public bool CanAfford(float amount)
        {
            return Balance >= amount;
        }
    }
}