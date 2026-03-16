using UnityEngine;

namespace Joker.Monopoly
{
    public class PlayerToken : MonoBehaviour
    {
        public void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }
    }
}