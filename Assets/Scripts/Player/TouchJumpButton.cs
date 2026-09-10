using System.Net.Security;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Player
{
    public class TouchJumpButton : MonoBehaviour, IPointerDownHandler
    {
        public bool JumpPressed { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            JumpPressed = true;
        }

        public void ConsumeJump()
        {
            JumpPressed = false;
        }
    }
}