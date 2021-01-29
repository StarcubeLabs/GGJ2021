namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DashVisualIndicator : MonoBehaviour
    {
        public List<SpriteRenderer> sprites;
        public Color defaultColor, dimColor;

        private void Update()
        {
            ToggleColor(PlayerController.instance.playerPhysics.canDash);
        }

        public void ToggleColor(bool canDash)
        {
            if (canDash)
            {
                sprites.ForEach(p => p.color = defaultColor);
            }
            else
            {
                sprites.ForEach(p => p.color = dimColor);
            }
        }
    }
}
