namespace GGJ2021.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MenuHandlerExtension : MonoBehaviour
    {
        public GameObject topSelection;
        public GameObject bottomSelection;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject != null) {
                return;
            }

            float inputChangeY = RewiredPlayerInputManager.instance.GetVerticalMovement();
            if (inputChangeY > 0) {
                EventSystem.current.SetSelectedGameObject(bottomSelection);
            }
            if (inputChangeY < 0) {
                EventSystem.current.SetSelectedGameObject(topSelection);
            }
        }
    }
}
