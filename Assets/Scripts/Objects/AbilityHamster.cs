namespace GGJ2021 {

    using UnityEngine;

    /// <summary>
    /// A hamster that unlocks an ability for the player.
    /// </summary>
    public class AbilityHamster : Collectible
    {
        [Tooltip("The ability given by the hamster.")]
        [SerializeField]
        private Ability ability;

        protected override void OnCollect()
        {
            if (ability == Ability.Grenade)
            {
                PlayerController.instance.cannon.SetActive(true);
            }
            FollowerManager.instance.AddAbilityHamster(ability);
        }

        public override void InitFromTilemap(Tilemap3D tilemap)
        {
            id = ability.ToString();
        }
    }
}