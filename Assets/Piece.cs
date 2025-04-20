using UnityEngine;

public class Piece : MonoBehaviour
{ 
    public SpriteRenderer spriteRenderer;

    private SpriteLib spriteLib;

    public Vector2Int coordinates;

    public Faction faction;

    public PieceBase piece;

    public BattleManager battleManager;

    private BoxCollider2D boxCollider;
    
    public void New(Vector2Int coords, Faction fac, PieceBase p, BattleManager batMan)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        battleManager = batMan;

        coordinates = coords;
        faction = fac;
        piece = p;

        spriteLib = battleManager.GetComponent<SpriteLib>();

        spriteRenderer.sprite = fac == Faction.player ? spriteLib.spritesP[piece.sprite] : spriteLib.spritesE[piece.sprite];
        transform.position = new Vector3 (coords.x - 3.5f, 4.8125f - coords.y * 0.75f,-1-coords.y);
    }

    private void OnMouseDown()
    {
        battleManager.ClickedPiece(this);
    }

    public void Grab()
    {
        spriteRenderer.sprite = spriteLib.spritesPSmall[piece.sprite];
        spriteRenderer.sortingOrder = 5;
        boxCollider.enabled = false;
    }

    public void Place(Vector2Int coords)
    {
        spriteRenderer.sprite = spriteLib.spritesP[piece.sprite];
        spriteRenderer.sortingOrder = 1;
        boxCollider.enabled = true;

        coordinates = coords;
        transform.position = new Vector3(coords.x - 3.5f, 4.8125f - coords.y * 0.75f, -1 - coords.y);
    }

    public void Return()
    {
        spriteRenderer.sprite = spriteLib.spritesP[piece.sprite];
        spriteRenderer.sortingOrder = 1;
        boxCollider.enabled = true;

        transform.position = new Vector3(coordinates.x - 3.5f, 4.8125f - coordinates.y * 0.75f, -1 - coordinates.y);
    }
}
