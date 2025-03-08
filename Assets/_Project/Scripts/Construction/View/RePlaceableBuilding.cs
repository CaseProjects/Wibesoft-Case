using Helpers.Utilities;
using UnityEngine;

namespace _Project.Scripts
{
    public class RePlaceableBuilding : Building
    {
        private bool _isMouseDown;
        private float _mouseDownTime;

        private void SetupBeforeMove()
        {
            _spriteRenderer.color = new Color(255, 255, 255, 0.5f);
        }

        private void OnMovementCancel()
        {
            _spriteRenderer.color = new Color(255, 255, 255, 1);
        }

        private protected void ResetMouseDown()
        {
            _mouseDownTime = 0;
            _isMouseDown = false;
        }

        protected override void OnMouseDown()
        {
            if (UIUtilities.IsFingerOverUI())
                return;

            _isMouseDown = true;
        }

        protected override void OnMouseOver()
        {
            if (_isMouseDown)
            {
                _mouseDownTime += Time.deltaTime;
            }
        }

        protected override void OnMouseDrag()
        {
            if (_buildState == BuildState.Idle && _isMouseDown && _mouseDownTime >= 0.1f)
            {
                _buildState = BuildState.Moving;
                _isMouseDown = false;
                _buildingSystem.SetupBuilding(this);
                SetupBeforeMove();
            }

            if (_buildState == BuildState.Moving)
            {
                MoveBuilding();
            }
        }

        protected override void OnMouseUp()
        {
            ResetMouseDown();
            if (_buildState == BuildState.Moving)
            {
                OnMouseUpWhenMovement();
            }
        }

        private protected void OnMouseUpWhenMovement()
        {
            if (IsPlaceable())
            {
                _buildingSystem.Confirm();
            }
            else
            {
                OnMovementCancel();
                _buildingSystem.CancelMove();
            }

            _buildState = BuildState.Idle;
        }
    }
}