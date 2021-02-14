using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManager : Singleton<BowManager>
{
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Queue<Arrow> arrowPool = new Queue<Arrow>();
    [SerializeField] private Transform arrowParent;
    [SerializeField] private int poolStartSize = 10;
    [SerializeField] private ParticleSystem particleSystem;

    Camera cam;
    private Arrow arrow;
    private AudioManager audioManager;
    public GameObject centerPoint;
    public Trajectory trajectory;
    [SerializeField] float pushForce = 4f;
    bool isDragging = false;

    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    Vector3 bowPosition;
    float distance;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Start()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            Arrow arrow1 = Instantiate(arrowPrefab);
            arrow1.transform.parent = arrowParent;
            arrowPool.Enqueue(arrow1);
            arrow1.gameObject.SetActive(false);
        }

        cam = Camera.main;
        bowPosition = centerPoint.transform.position;
    }

    void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnDragEnd();
            }
            if (isDragging)
            {
                OnDrag();
            }
        }
       
    }
    public Arrow GetArrow()
    {
        if (arrowPool.Count > 0)
        {
            Arrow arrow1 = arrowPool.Dequeue();
            arrow1.gameObject.SetActive(true);
            return arrow1;
        }
        else
        {
            Arrow arrow1 = Instantiate(arrowPrefab);
            arrow1.transform.parent = arrowParent;
            return arrow1;
        }
    }

    public void ReturnArrow(Arrow arrow1)
    {
        arrowPool.Enqueue(arrow1);
        arrow1.gameObject.SetActive(false);
    }
    void OnDragStart()
    {
        arrow = GetArrow();
        arrow.transform.position = bowPosition;
        arrow.DeactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();
    }

    void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;


        arrow.transform.right = direction;
        centerPoint.transform.right = direction;
        
        Debug.DrawLine(startPoint, endPoint);

        trajectory.UpdateDots(arrow.pos, force);
    }

    void OnDragEnd()
    {
        arrow.ActivateRb();
        arrow.Push(force);
        PlaySound("Arrow");
        trajectory.Hide();
    }

    public void PlayParticle(Transform location)
    {
        particleSystem.transform.position = location.position;
        particleSystem.Play();
    }

    public void PlaySound(string name)
    {
        audioManager.Play(name);
    }
}
