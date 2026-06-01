using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Editor
{
    public interface IDragableObject
    {
        public void OnDragStarted();
    }
    public class DragManipulator : PointerManipulator
    {
        protected bool _active = false;
        protected Vector3 _startPosition;
        protected int _pointerId;
       

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
           
            target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        }

        private void PointerUpHandler(PointerUpEvent evt)
        {
            Debug.Log("Pointer Up");

        }



        private void PointerDownHandler(PointerDownEvent evt)
        {
            Debug.Log("Pointer Down");

           

            var dragObject = target as IDragableObject;
            if (dragObject == null)
            {
                Debug.LogError("Target is not Dragable Object");
                return;
            }

            _pointerId = evt.pointerId;
                   
            dragObject.OnDragStarted();
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
     
            target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        }
    }
}