namespace GGJ2021.UserInterface
{

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class HealthBarHandler : MonoBehaviour
  {

      public int healthMax;
      private int healthCurr;

      public GameObject healthIcon;
      private float iconWidth = 25;

      // Start is called before the first frame update
      void Start()
      {
        // init values
        healthCurr = healthMax;

        InstantiateIcons();
      }

      public void InstantiateIcons() {
        for (int ii=0; ii<healthMax; ii++) {
          // InstantiateIcon(ii);
          StartCoroutine(InstantiateIcon(ii));
        }
      }

      IEnumerator InstantiateIcon(int idx) {
        GameObject newIcon = Instantiate(healthIcon, transform);

        Vector3 newPosition = transform.position;
        newPosition.x += idx * (iconWidth + 10);
        newIcon.transform.position = newPosition;

        // trying to stagger the animation but it's stupid
        float timeOffset = (float)(idx * 0.5);
        GameObject healthFull = newIcon.transform.Find("Health_Full").gameObject;
        Animator iconAnim = healthFull.GetComponent<Animator>();
        iconAnim.enabled = false;
        yield return new WaitForSeconds(timeOffset);
        iconAnim.enabled = true;
      }

      public void IncrementMaxHealth(int amount) {
        int newMax = healthMax + amount;

        // first create additional icons
        for (int ii=healthMax; ii<newMax; ii++) {
          StartCoroutine(InstantiateIcon(ii));
        }

        // then we can add health, can do curr=max another time
        healthMax = newMax;
        healthCurr = healthCurr + amount;
      }
  }
}
