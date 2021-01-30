﻿namespace GGJ2021.UI
{

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class StatBarManager : MonoBehaviour
  {
      public static StatBarManager instance;

      public int statMax;
      private int statCurr;

      public GameObject iconObject;
      private List<GameObject> icons;
      private float iconWidth = 25;

      private bool isInitialized;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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

        isInitialized = true;
        RedrawIcons();
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

      void RedrawIcons() {
        if (!isInitialized) return;

        for(int ii=0; ii<statMax; ii++) {
          bool newIconState = ii < statCurr;
          ToggleIconState(ii, newIconState);
        }
      }

      public void SetMax(int newMax) {

        // first create additional icons
        for (int ii=statMax; ii<newMax; ii++) {
          StartCoroutine(InstantiateIcon(ii));
        }

        // then we can add health, can do curr=max if we wanna
        statMax = newMax;
        statCurr = newMax;
        RedrawIcons();
      }

      public void SetCurr(int newCurr) {
        statCurr = newCurr;
        RedrawIcons();
      }
  }
}
