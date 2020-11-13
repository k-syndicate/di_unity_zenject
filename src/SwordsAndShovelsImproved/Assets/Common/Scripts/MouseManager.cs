using System;
using UnityEngine;

public class MouseManager : MonoBehaviour, IMouseService
{
    public LayerMask clickableLayer;

    public Texture2D pointer;
    public Texture2D target;
    public Texture2D doorway;
    public Texture2D sword;

    public event Action<Vector3> OnEnvironmentClick;
    public event Action<Vector3> OnEnvironmentRightClick;
    public event Action<GameObject> OnAttackableClick;

    private bool _useDefaultCursor = false;

    private void Awake()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void Update()
    {
        // Set cursor
        Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        if (_useDefaultCursor)
        {
            return;
        }

        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
        {
            // Override cursor
            Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);

            bool door = false;
            if (hit.collider.gameObject.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }

            bool chest = false;
            if (hit.collider.gameObject.tag == "Chest")
            {
                Cursor.SetCursor(pointer, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }

            bool isAttackable = hit.collider.GetComponent(typeof(IAttackable)) != null;
            if(isAttackable)
            {
                Cursor.SetCursor(sword, new Vector2(16, 16), CursorMode.Auto);
            }
            // If environment surface is clicked, invoke callbacks.
            if (Input.GetMouseButtonDown(0))
            {
                if (door)
                {
                    Transform doorway = hit.collider.gameObject.transform;
                    OnEnvironmentClick?.Invoke(doorway.position + doorway.forward * 10);
                }
                else if(isAttackable)
                {
                    GameObject attackable = hit.collider.gameObject;
                    OnAttackableClick?.Invoke(attackable);
                }
                else if (!chest)
                {
                    OnEnvironmentClick?.Invoke(hit.point);
                }
            }
            else if(Input.GetMouseButtonDown(1))
            {
                if(!door && !chest)
                {
                    OnEnvironmentRightClick?.Invoke(hit.point);
                }
            }
        }
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _useDefaultCursor = (currentState != GameManager.GameState.RUNNING);
    }
}
