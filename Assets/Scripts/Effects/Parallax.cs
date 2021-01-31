namespace GGJ2021.Effects
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public class Parallax : MonoBehaviour
    {
        public Transform target;
        public InfiniteScroll[] layers;
        public float[] layersVelocityScales;
        public Vector2 parallaxEffectMulitplyer;

        private Vector2 targetVector;
        public Vector2 currentOffset;

        private void Start()
        {
            for (int i = 0; i < layers.Length; i++)
                Assert.IsNotNull(layers[i]);
            Assert.AreEqual(layers.Length, layersVelocityScales.Length);

            targetVector = Vector2.zero;
            currentOffset = Vector2.zero;
        }

        private void Update()
        {
            if (target == null)
            {
                if (PlayerController.instance == null)
                    return;

                target = PlayerController.instance.transform;
            }

            Vector2 temp = (this.transform.position - target.position).normalized;
            temp = Vector2.Scale(temp, parallaxEffectMulitplyer);
            float x = temp.x;
            float y = temp.y;
            targetVector = new Vector2(x > .2f ? x : targetVector.x, y > .2f ? y : targetVector.y);
            if (currentOffset == Vector2.zero)
                currentOffset = targetVector;
            else
                currentOffset = Vector2.MoveTowards(currentOffset, targetVector, Time.deltaTime);
            
            Vector2 vel = currentOffset;
            Vector2 foreground = vel * layersVelocityScales[0];
            Vector2 middleground = vel * layersVelocityScales[1];
            Vector2 background = vel * layersVelocityScales[2];

            layers[0].transform.localPosition = new Vector3(foreground.x, foreground.y, layers[0].transform.localPosition.z);
            layers[1].transform.localPosition = new Vector3(middleground.x, middleground.y, layers[1].transform.localPosition.z);
            layers[2].transform.localPosition = new Vector3(background.x, background.y, layers[2].transform.localPosition.z);
        }
    }
}
