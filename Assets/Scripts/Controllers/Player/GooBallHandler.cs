namespace GGJ2021
{
    using UnityEngine;

    public class GooBallHandler : MonoBehaviour
    {
        public SpriteRenderer sprite;
        public float rotationSpeed;

        // Update is called once per frame
        void Update()
        {
            if (PlayerController.instance.facingRight)
            {
                sprite.transform.eulerAngles = new Vector3(sprite.transform.eulerAngles.x, sprite.transform.eulerAngles.y, sprite.transform.eulerAngles.z - rotationSpeed);
            }
            else
            {
                sprite.transform.eulerAngles = new Vector3(sprite.transform.eulerAngles.x, sprite.transform.eulerAngles.y, sprite.transform.eulerAngles.z + rotationSpeed);
            }
        }

        public void ShowBall()
        {
            sprite.enabled = true;
        }

        public void HideBall()
        {
            sprite.enabled = false;
        }

        public void StartDash()
        {
            PlayerController.instance.astronautSprites.ForEach(p => p.enabled = false);
            ShowBall();
        }

        public void StopDash()
        {
            PlayerController.instance.astronautSprites.ForEach(p => p.enabled = true);
            HideBall();
        }
    }
}
