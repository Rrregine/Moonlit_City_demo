using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private RectTransform cursor;

    //Cursor Image
    [SerializeField]
    private Image cursorImage;

    [SerializeField]
    private Sprite normalCursor;

    [SerializeField]
    private Sprite swordCursor;

    [SerializeField]
    private Sprite eyeCursor;

    //Cursor Location
    [SerializeField]
    private Vector2 cursorOffset;

    private void Start()
    {
        Cursor.visible = false;
        //Debug.Log("Cursor Start");
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        cursor.position = mousePosition + cursorOffset;
    }


    public void SetNormalCursor()
    {
        cursorImage.sprite = normalCursor;
    }

    public void SetSwordCursor()
    {
        cursorImage.sprite = swordCursor;
    }

    public void SetEyeCursor()
    {
        cursorImage.sprite = eyeCursor;
    }
}
