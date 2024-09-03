using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Hazard_Related
{
    public class BlackOutWarningTrigger : MonoBehaviour
    {
        private RectTransform _imgTransform;

        [Header("Image Size Values")] 
        [SerializeField] private float width = 100f;
        [SerializeField] private float height = 100f;
        
        [Header("Position Values")]
        [SerializeField] private float xOffset = 0f;
        [SerializeField] private float yOffset = 0f;

        public void InitializeImage()
        {
            _imgTransform = GetComponent<RectTransform>();
            _imgTransform.sizeDelta = new Vector2(width, height);
            
            var position = _imgTransform.position;
            position = new Vector3(position.x + xOffset, position.y + yOffset);
            _imgTransform.position = position;
        }

        public void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}