using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PieceBase
{
    public string name;

    public string description;

    public int sprite;

    public virtual bool[,] moves(List<Piece> pieces, Vector2Int coords, Faction fac) { return new bool[8, 8]; }

    public virtual bool[,] attacks(List<Piece> pieces, Vector2Int coords, Faction fac) { return new bool[8, 8]; }
}

public class Pawn : PieceBase
{
    public Pawn()
    {
        name = "Pawn";

        description = "The weakest piece";

        sprite = 0;
    }

    public override bool[,] moves(List<Piece> pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        bool spotFree = true;
        foreach (Piece piece in pieces)
        {
            if (piece.coordinates == (coords + (fac == Faction.player ? new Vector2Int(1, 0) : new Vector2Int(-1, 0))))
            {
                spotFree = false;
                break;
            }
        }
        if (spotFree)
        {
            possibleMoves[coords.x + (fac == Faction.player ? 1 : -1), coords.y] = true;

            if (coords.x < 2)
            {
                foreach (Piece piece in pieces)
                {
                    if (piece.coordinates == (coords + (fac == Faction.player ? new Vector2Int(2, 0) : new Vector2Int(-2, 0))))
                    {
                        spotFree = false;
                        break;
                    }
                }

                possibleMoves[coords.x + (fac == Faction.player ? 2 : -2), coords.y] = true;
            }
        }

        return possibleMoves;
    }
    public override bool[,] attacks(List<Piece> pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleAttacks = new bool[8, 8];

        bool spotFree = true;
        foreach (Piece piece in pieces)
        {
            if (piece.coordinates == (coords + (fac == Faction.player ? new Vector2Int(1, 1) : new Vector2Int(-1, 1))) && piece.faction != fac)
            {
                spotFree = false;
                break;
            }
        }
        if (!spotFree)
        {
            possibleAttacks[coords.x + (fac == Faction.player ? 1 : -1), coords.y + 1] = true;
        }

        spotFree = true;
        foreach (Piece piece in pieces)
        {
            if (piece.coordinates == (coords + (fac == Faction.player ? new Vector2Int(1, -1) : new Vector2Int(-1, -1))) && piece.faction != fac)
            {
                spotFree = false;
                break;
            }
        }
        if (!spotFree)
        {
            possibleAttacks[coords.x + (fac == Faction.player ? 1 : -1), coords.y - 1] = true;
        }

        return possibleAttacks;
    }
}