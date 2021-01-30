namespace GGJ2021.Effects
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public class InfiniteScroll : MonoBehaviour
    {
        public GameObject Image;

        public Transform OriginPoint;
        public Transform OffsetPoint;
        public Transform SwapPointLeft;
        public Transform SwapPointRight;

        public bool AutoScroll;
        public bool ScrollRight;
        public float ScrollSpeed;
        
        private GameObject[] images;

        private void Start()
        {
            Assert.IsNotNull(Image);
            Assert.IsNotNull(OffsetPoint);
            Assert.IsNotNull(SwapPointLeft);
            Assert.IsNotNull(SwapPointRight);

            if (AutoScroll)
                Assert.IsTrue(ScrollSpeed > 0);

            images = new GameObject[2];
            images[0] = Instantiate(Image);
            images[1] = Instantiate(Image);
            images[0].transform.parent = this.transform;
            images[1].transform.parent = this.transform;

            if (OriginPoint != null)
                images[0].transform.localPosition = new Vector3(OriginPoint.localPosition.x, OriginPoint.localPosition.y, 0);
            else
                images[0].transform.localPosition = new Vector3(0, 0, 0);

            images[1].transform.localPosition = new Vector3(OffsetPoint.localPosition.x, OffsetPoint.localPosition.y, 0);
        }

        private void Update()
        {
            if (AutoScroll)
            {
                Transform point = ScrollRight ? SwapPointRight : SwapPointLeft;
                Transform opposite = ScrollRight ? SwapPointLeft : SwapPointRight;
                foreach (GameObject image in images)
                {
                    if (Vector2.Distance(image.transform.localPosition, point.transform.localPosition) < .1)
                        image.transform.localPosition = new Vector3(opposite.localPosition.x, opposite.localPosition.y, image.transform.localPosition.z);
                    else
                        image.transform.localPosition = Vector3.MoveTowards(image.transform.localPosition,
                            new Vector3(point.localPosition.x, point.localPosition.y, image.transform.localPosition.z), ScrollSpeed * Time.deltaTime);
                }
            }
        }
    }
}
