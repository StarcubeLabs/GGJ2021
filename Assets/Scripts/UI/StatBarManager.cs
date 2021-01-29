namespace GGJ2021.UserInterface
{

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class HealthBarHandler : MonoBehaviour
  {

      public int statMax;
      private int statCurr;

      public GameObject iconObject;
      private List<GameObject> icons;
      private float iconWidth = 25;

      void Start()
      {
        statCurr = statMax;
        icons = new List<GameObject>();

        InitIcons();
      }

      public void InitIcons() {
        for (int ii=0; ii<statMax; ii++) {
          StartCoroutine(InstantiateIcon(ii));
        }
      }

      IEnumerator InstantiateIcon(int idx) {
        GameObject newIcon = Instantiate(iconObject, transform);

        Vector3 newPosition = transform.position;
        newPosition.x += idx * (iconWidth + 10);
        newIcon.transform.position = newPosition;

        // trying to stagger the animation but it's stupid
        float timeOffset = (float)(idx * 0.5);
        Animator iconAnim = newIcon.GetComponent<Animator>();
        iconAnim.enabled = false;
        yield return new WaitForSeconds(timeOffset);
        iconAnim.enabled = true;
      }

      public void AddMax(int amount) {
        int newMax = statMax + amount;

        // first create additional icons
        for (int ii=statMax; ii<newMax; ii++) {
          StartCoroutine(InstantiateIcon(ii));
        }

        // then we can add health, can do curr=max another time
        statMax = newMax;
        statCurr = statCurr + amount;
      }
  }
}
