namespace GGJ2021
{
    using UnityEngine;

    public class DustToggle : MonoBehaviour
    {
        public ParticleSystem particles;

        // Update is called once per frame
        void Update()
        {
            if (PlayerStats.instance.HasAbility(Ability.Dash))
            {
                if (!particles.isEmitting)
                {
                    particles.Play();
                }
            }
            else
            {
                if (particles.isEmitting)
                {
                    particles.Stop();
                }
            }
        }
    }
}
