namespace GGJ2021
{
    using UnityEngine;

    public class AimingReticule : MonoBehaviour
    {
        public float maxDistance;
        [HideInInspector]
        public float currentDistance;

        private Color initColor, fadedColor;

        [SerializeField]
        private SpriteRenderer sprite;

        void Start()
        {
            initColor = sprite.color;
            fadedColor = sprite.color;
            fadedColor.a = 0f;
        }

        void Update()
        {
            sprite.color = Color.Lerp(fadedColor, initColor, PlayerController.instance.config.reticuleCurve.Evaluate(currentDistance / maxDistance));
        }
    }
}
