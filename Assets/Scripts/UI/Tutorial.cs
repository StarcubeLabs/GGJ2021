namespace GGJ2021.UI
{
    using UnityEngine;

    public class Tutorial : MonoBehaviour
    {
        public GameObject tutorial;
        public bool checkForAbility;
        public Ability ability;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (checkForAbility)
            {
                if (!PlayerStats.instance.HasAbility(ability))
                    return;
            }

            tutorial.SetActive(true);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (checkForAbility)
            {
                if (!PlayerStats.instance.HasAbility(ability))
                    return;
            }

            tutorial.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (checkForAbility)
            {
                if (!PlayerStats.instance.HasAbility(ability))
                    return;
            }

            tutorial.SetActive(false);
        }
    }
}
