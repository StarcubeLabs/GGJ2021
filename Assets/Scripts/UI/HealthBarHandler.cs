namespace GGJ2021.UserInterface
{

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class HealthBarHandler : MonoBehaviour
  {

      public int healthMax;

      public int healthCurr;

      public GameObject healthIcon;
      private float iconWidth = 25;

      // Start is called before the first frame update
      void Start()
      {
        // init values
        healthCurr = healthMax;

        this.InstantiateIcons();
      }

      public void InstantiateIcons() {
        for (int ii=0; ii<healthMax; ii++) {
          InstantiateIcon(ii);
        }
      }

      public void InstantiateIcon(int idx) {
        GameObject icon = Instantiate(healthIcon, transform);

        Vector3 newPosition = transform.position;
        newPosition.x += idx * (iconWidth + 10);

        icon.transform.position = newPosition;
      }

      public void IncrementMaxHealth(int amount) {
        int newMax = healthMax + amount;

        // first create additional icons
        for (int ii=healthMax; ii<newMax; ii++) {
          InstantiateIcon(ii);
        }

        // then we can add health, can do curr=max another time
        healthMax = newMax;
        healthCurr = healthCurr + amount;
      }
  }
}
