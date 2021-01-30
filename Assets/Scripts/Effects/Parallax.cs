namespace GGJ2021.Effects
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public class Parallax : MonoBehaviour
    {
        public Transform target;
        public InfiniteScroll[] layers;
        public float parallaxEffectMulitplyer;

        private void Start()
        {
            Assert.IsNotNull(target);
            for (int i = 0; i < layers.Length; i++)
                Assert.IsNotNull(layers[i]);
        }

        private void Update()
        {
            Vector2 vel = (this.transform.position - target.position).normalized * parallaxEffectMulitplyer;
            Vector2 foreground = vel * .25f;
            Vector2 middleground = vel * .5f;
            Vector2 background = vel;

            layers[0].transform.localPosition = new Vector3(foreground.x, foreground.y, layers[0].transform.localPosition.z);
            layers[1].transform.localPosition = new Vector3(middleground.x, middleground.y, layers[1].transform.localPosition.z);
            layers[2].transform.localPosition = new Vector3(background.x, background.y, layers[2].transform.localPosition.z);
        }
    }
}
