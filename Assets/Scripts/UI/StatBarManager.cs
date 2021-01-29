namespace GGJ2021.UI
{

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class StatBarManager : MonoBehaviour
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
        icons.Add(newIcon);

        Vector3 newPosition = transform.position;
        newPosition.x += idx * (iconWidth + 10);
        newIcon.transform.position = newPosition;

        // trying to stagger the animation but it's stupid
        float timeOffset = (float)(idx * 0.2);
        Animator iconAnim = newIcon.GetComponent<Animator>();
        iconAnim.enabled = false;
        yield return new WaitForSeconds(timeOffset);
        iconAnim.enabled = true;

        ToggleIconState(newIcon, true);
      }

      void ToggleIconState(GameObject icon, bool isEnabled) {
        GameObject activeIcon = icon.transform.GetChild(1).gameObject;
        activeIcon.SetActive(isEnabled);

        Animator iconAnim = icon.GetComponent<Animator>();
        iconAnim.enabled = isEnabled;
      }

      void ToggleIconState(int idx, bool isEnabled) {
        GameObject icon = icons[idx];
        ToggleIconState(icon, isEnabled);
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

      public void SubtractCurr(int amount) {
        int newCurr = statCurr -= amount;

        // for (int ii=statCurr; ii>0; ii--) {
        for(int ii=newCurr; ii<statMax; ii++) {
          print("ii " + ii);
          ToggleIconState(ii, false);
        }
      }
  }
}
