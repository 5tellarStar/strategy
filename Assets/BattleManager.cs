using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;

    public bool Playerturn;

    public List<Piece> pieces = new();

    private void Start()
    {

        AddPiece(new Vector2Int(0, 0), Faction.player, new Pawn());
        AddPiece(new Vector2Int(0, 1), Faction.player, new Pawn());
        AddPiece(new Vector2Int(0, 2), Faction.player, new Pawn());
    }

    private void AddPiece(Vector2Int coords, Faction fac, PieceBase p)
    {
        Piece piece = Instantiate(piecePrefab).GetComponent<Piece>();
        piece.New(coords, fac, p, this);
        pieces.Add(piece);
    }
}

public enum Faction
{
    player,
    enemy
}