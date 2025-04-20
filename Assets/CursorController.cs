using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    public Piece heldPiece;

    [SerializeField] private Sprite pointingSprite;
    [SerializeField] private Sprite grabingSprite;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = Input.mousePosition * Camera.main.orthographicSize * 2 / Screen.height - new Vector3(Camera.main.orthographicSize * Camera.main.aspect - 1f / 16f, Camera.main.orthographicSize + 4f / 16f);
        if (heldPiece != null)
        {
            heldPiece.transform.position = transform.position;
        }
    }

    public void Grab(Piece piece)
    {
        heldPiece = piece;

        spriteRenderer.sprite = grabingSprite;
    }

    public void LetGo()
    {
        heldPiece = null;

        spriteRenderer.sprite = pointingSprite;
    }
}
