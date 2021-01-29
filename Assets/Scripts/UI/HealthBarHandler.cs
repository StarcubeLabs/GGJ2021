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

      void InstantiateIcons() {
        for (int ii=0; ii<healthMax; ii++) {
          InstantiateIcon(ii);
        }
      }

      void InstantiateIcon(int idx) {
        GameObject icon = Instantiate(healthIcon, transform);

        Vector3 newPosition = transform.position;
        newPosition.x += idx * (iconWidth + 10);

        icon.transform.position = newPosition;
      }
  }
}
