using System;
using UnityEngine;

namespace Joker.Monopoly
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private InventorySO inventory;

        private void Awake()
        {
            if(Instance !=  null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            if (inventory == null) return;


            inventory.LoadAllItems();
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

    }
}

