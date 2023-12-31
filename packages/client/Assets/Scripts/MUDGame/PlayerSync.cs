using UnityEngine;

namespace mudworld
{
    public class PlayerSync : MonoBehaviour
    {
        [HideInInspector] public string key;
        public static string? localPlayerKey;
        
        public bool IsLocalPlayer()
        {
            return localPlayerKey != null && localPlayerKey == key;
        }
    }
}
