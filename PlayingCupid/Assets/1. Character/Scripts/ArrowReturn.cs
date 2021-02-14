using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowReturn : MonoBehaviour
{
    private BowManager bowManager;
    private Arrow thisArrow;
    private void Awake()
    {
        thisArrow = this.gameObject.GetComponent<Arrow>();
    }
    // Start is called before the first frame update
    void Start()
    {
        bowManager = FindObjectOfType<BowManager>();
    }

    private void OnDisable()
    {
        if(bowManager != null)
            bowManager.ReturnArrow(thisArrow);
    }
}
