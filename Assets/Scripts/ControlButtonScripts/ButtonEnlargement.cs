using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace ControlButtonScripts
{
    public class ButtonEnlargement : MonoBehaviour
    {
        private void Start()
        {
            EventTrigger evTrig = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry pointerDownEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDownEvent.callback.AddListener(IncreaseBtnSize);
            EventTrigger.Entry pointerUpEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerUp
            };
            pointerUpEvent.callback.AddListener(DecreaseBtnSize);
            evTrig.triggers.Add(pointerDownEvent);
            evTrig.triggers.Add(pointerUpEvent);
        }

        private void IncreaseBtnSize(BaseEventData eventData)
        {
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f);
        }

        public void DecreaseBtnSize(BaseEventData eventData)
        {
            gameObject.transform.localScale = new Vector3(1f, 1f);
        }

    }
}