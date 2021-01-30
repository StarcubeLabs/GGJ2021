namespace GGJ2021
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DashVisualIndicator : MonoBehaviour
    {
        public List<SpriteRenderer> sprites;
        public Color defaultColor, dimColor;
        private Color tempColor;

        private void Update()
        {
            ToggleColor(PlayerController.instance.playerPhysics.CanDash(), PlayerController.instance.playerHealth.IsInvulnerableFromHit());
        }

        public void ToggleColor(bool canDash, bool isInvulnerableFromHit)
        {
            if (canDash)
            {
                sprites.ForEach(p => p.color = defaultColor);
            }
            else
            {
                sprites.ForEach(p => p.color = dimColor);
            }
            if (isInvulnerableFromHit)
            {
                sprites.ForEach(p =>
                {
                    tempColor = p.color;
                    tempColor.a = 0.5f;
                    p.color = tempColor;
                });
            }
            else
            {
                sprites.ForEach(p =>
                {
                    tempColor = p.color;
                    tempColor.a = 1.0f;
                    p.color = tempColor;
                });
            }
        }
    }
}
