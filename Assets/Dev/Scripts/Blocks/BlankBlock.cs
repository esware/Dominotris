using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Scripts.Tiles
{
    public class BlankBlock:MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            StartCoroutine(LerpColor());
        }
        

        IEnumerator LerpColor()
        {
            GetComponent<Image>().DOColor(Color.white *0.6f, .5f);
            yield return new WaitForSeconds(0.3f);
            GetComponent<Image>().DOColor(Color.white, .5f);
        }
    }
}