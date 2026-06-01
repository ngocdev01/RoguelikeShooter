using System;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Editor
{
    public class DropManipulator : PointerManipulator
    {
        protected bool _active = false;
        protected int _pointerId;
        protected Func<bool> _canAcceptDrop;
        protected Action _onDragPerform;

        public DropManipulator(Func<bool> canAcceptDrop = null, Action onDragPerform = null)
        {
            _canAcceptDrop = canAcceptDrop;
            _onDragPerform = onDragPerform;
        }
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<DragEnterEvent>(OnDragEnter);
            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
            target.RegisterCallback<DragPerformEvent>(OnDragPerform);
            target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
        }

        private void OnDragLeave(DragLeaveEvent evt)
        {

        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            _onDragPerform?.Invoke();
        }

        private void OnDragUpdate(DragUpdatedEvent evt)
        {

            if (_canAcceptDrop != null && _canAcceptDrop.Invoke())
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
        }

        void OnDragEnter(DragEnterEvent _)
        {





        }
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);

        }
    }
}