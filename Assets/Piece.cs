using UnityEngine;

public class Piece : MonoBehaviour
{ 
    private SpriteRenderer spriteRenderer;

    public Vector2Int coordinates;

    public Faction faction;

    public PieceBase piece;

    public BattleManager battleManager;

    
    public void New(Vector2Int coords, Faction fac, PieceBase p, BattleManager batMan)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        battleManager = batMan;

        coordinates = coords;
        faction = fac;
        piece = p;

        spriteRenderer.sprite = fac == Faction.player ? battleManager.GetComponent<SpriteLib>().spritesP[piece.sprite] : battleManager.GetComponent<SpriteLib>().spritesE[piece.sprite];
        transform.position = new Vector3 (coords.x - 3.5f, 4.8125f - coords.y * 0.75f,7-coords.y);
    }

    private void OnMouseDown()
    {
        Debug.Log(coordinates);
    }
}
