using System;
using _Main.ToolKit.SingletonFeature;
using Main.InputSys.Sys.Event;
using MessagePipe;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.InputSys.Sys
{
    [Serializable]
    public class InputSystem : Singleton<InputSystem>
    {
        [SerializeField] private GameInputControl gameInputControl;
        [SerializeField] private GameInputControl.PlayerActions playerActions;
        
        #region Life Cycle

        protected override void Initialize()
        {
            SetupNewInput();
            SetupInputEvent();
        }
        
        private void SetupNewInput()
        {
            gameInputControl = new GameInputControl();
            gameInputControl.Enable();
            playerActions = gameInputControl.Player;
            playerActions.Enable();
        }

        protected override void Release()
        {
            LockInput();
            LockPlayerInput();
            gameInputControl.Dispose();
        }

        #endregion

        #region Enable & Disable

        private void LockInput()
        {
            gameInputControl.Disable();
        }

        private void UnlockInput()
        {
            gameInputControl.Enable();
        }
        
        private void LockPlayerInput()
        {
            playerActions.Disable();
        }
        
        private void UnlockPlayerInput()
        {
            playerActions.Enable();
        }

        #endregion

        #region Input

        private void SetupInputEvent()
        {
            ClickInput();
            PointInput();
        }

        #region Click

        private void ClickInput()
        {
            playerActions.Click.started += ClickStarted;
            playerActions.Click.performed += ClickStay;
            playerActions.Click.canceled += ClickCanceled;
        }

        private void ClickStarted(InputAction.CallbackContext obj)
        {
            Vector2 clickPosition = playerActions.Point.ReadValue<Vector2>();
            Event_InputOnHit eventData = new Event_InputOnHit(clickPosition);
            GlobalMessagePipe.GetPublisher<Event_InputOnHit>().Publish(eventData);
        }
        
        private void ClickStay(InputAction.CallbackContext obj)
        {
        }

        private void ClickCanceled(InputAction.CallbackContext obj)
        {
        }

        #endregion
        
        private void PointInput()
        {
            
        }

        #endregion
    }
}