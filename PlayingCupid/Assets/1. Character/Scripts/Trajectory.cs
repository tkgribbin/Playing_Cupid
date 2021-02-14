using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] GameObject dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float dotSpacing;

    Transform[] dotsList;
    Vector3 pos;
    float spacing;


    void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        for (int i = 0; i < dotsNumber ; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;
        }
    }

    public void UpdateDots(Vector3 arrowPos, Vector2 forceApplied)
    {
        spacing = dotSpacing;
        for (int i = 0; i < dotsNumber; i++)
        {
            pos.x = (arrowPos.x + forceApplied.x * spacing);
            pos.y = (arrowPos.y + forceApplied.y * spacing) - (Physics2D.gravity.magnitude * spacing * spacing) / 2f;
            pos.z = dotsParent.transform.position.z;
            dotsList[i].position = pos;
            spacing += dotSpacing;
        }
    }

    public void Show()
    {
        dotsParent.SetActive(true);
    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        PrepareDots();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
