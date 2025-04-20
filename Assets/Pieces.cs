using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;


public class PieceBase
{
    public string name;

    public string description;

    public int sprite;

    public virtual bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac) { return new bool[8, 8]; }

    public virtual bool[,] attacks(Piece[,] pieces, Vector2Int coords, Faction fac) { return new bool[8, 8]; }
}

public class Pawn : PieceBase
{
    public Pawn()
    {
        name = "Pawn";

        description = "The weakest piece";

        sprite = 0;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.x != 7)
        {
            if (pieces[coords.x + (fac == Faction.player ? 1 : -1), coords.y] == null)
            {
                possibleMoves[coords.x + (fac == Faction.player ? 1 : -1), coords.y] = true;

                if ((coords.x == 1 && fac == Faction.player)||(coords.x == 6 && fac == Faction.enemy))
                {
                    if (pieces[coords.x + (fac == Faction.player ? 2 : -2), coords.y] == null)
                    {
                        possibleMoves[coords.x + (fac == Faction.player ? 2 : -2), coords.y] = true;
                    }
                }
            }
        }
        return possibleMoves;
    }
    public override bool[,] attacks(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleAttacks = new bool[8, 8];

        if (coords.y != 7)
        {
            if (pieces[coords.x + (fac == Faction.player ? 1 : -1), coords.y + 1] != null)
            {
                if (pieces[coords.x + (fac == Faction.player ? 1 : -1), coords.y + 1].faction != fac)
                {
                    possibleAttacks[coords.x + (fac == Faction.player ? 1 : -1), coords.y + 1] = true;
                }
            }
        }

        if (coords.y != 0)
        {
            if (pieces[coords.x + (fac == Faction.player ? 1 : -1), coords.y - 1] != null)
            {
                if (pieces[coords.x + (fac == Faction.player ? 1 : -1), coords.y - 1].faction != fac)
                {
                    possibleAttacks[coords.x + (fac == Faction.player ? 1 : -1), coords.y - 1] = true;
                }
            }
        }
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if(possibleAttacks[x,y])
                    Debug.Log(new Vector2Int(x, y));
            }
        }

        return possibleAttacks;
    }
}

public class Rook : PieceBase
{
    public Rook()
    {
        name = "Rook";

        description = "A powerful piece that moves in a + shape";

        sprite = 1;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.x != 7)
        {
            for(int x = coords.x + 1;x < 8;x++)
            {
                if (pieces[x,coords.y] == null)
                {
                    possibleMoves[x,coords.y] = true;
                }
                else
                {
                    break;
                }
            }
        }
        if (coords.x != 0)
        {
            for (int x = coords.x - 1; x > -1; x--)
            {
                if (pieces[x, coords.y] == null)
                {
                    possibleMoves[x, coords.y] = true;
                }
                else
                {
                    break;
                }
            }
        }
        if (coords.y != 7)
        {
            for (int y = coords.y + 1; y < 8; y++)
            {
                if (pieces[coords.x, y] == null)
                {
                    possibleMoves[coords.x, y] = true;
                }
                else
                {
                    break;
                }
            }
        }
        if (coords.y != 0)
        {
            for (int y = coords.y - 1; y > -1; y--)
            {
                if (pieces[coords.x, y] == null)
                {
                    possibleMoves[coords.x, y] = true;
                }
                else
                {
                    break;
                }
            }
        }

        return possibleMoves;
    }

    public override bool[,] attacks(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleAttacks = new bool[8, 8];

        if (coords.x != 7)
        {
            for (int x = coords.x + 1; x < 8; x++)
            {
                if (pieces[x, coords.y] != null)
                {
                    if(pieces[x, coords.y].faction != fac)
                    {
                        possibleAttacks[x, coords.y] = true;
                    }
                    break;
                }
            }
        }
        if (coords.x != 0)
        {
            for (int x = coords.x - 1; x > -1; x--)
            {
                if (pieces[x, coords.y]!= null)
                {
                    if (pieces[x, coords.y].faction != fac)
                    {
                        possibleAttacks[x, coords.y] = true;
                    }
                    break;
                }
            }
        }
        if (coords.y != 7)
        {
            for (int y = coords.y + 1; y < 8; y++)
            {
                if (pieces[coords.x, y] != null)
                {
                    if (pieces[coords.x, y].faction != fac)
                    {
                        possibleAttacks[coords.x, y] = true;
                    }
                    break;
                }
            }
        }
        if (coords.y != 0)
        {
            for (int y = coords.y - 1; y > -1; y--)
            {
                if (pieces[coords.x, y] != null)
                {
                    if (pieces[coords.x, y].faction != fac)
                    {
                        possibleAttacks[coords.x, y] = true;
                    }
                    break;
                }
            }
        }

        return possibleAttacks;
    }
}

public class Knight : PieceBase
{
    public Knight()
    {
        name = "Knight";

        description = "A piece that moves in a L shape able to jump over other pieces";

        sprite = 2;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if(coords.y < 6)
        {
            if(coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y + 2] == null)
                {
                    possibleMoves[coords.x + 1, coords.y + 2] = true;
                }
            }
            if(coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y + 2] == null)
                {
                    possibleMoves[coords.x - 1, coords.y + 2] = true;
                }
            }
        }

        if (coords.y > 1)
        {
            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y - 2] == null)
                {
                    possibleMoves[coords.x + 1, coords.y - 2] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y - 2] == null)
                {
                    possibleMoves[coords.x - 1, coords.y - 2] = true;
                }
            }
        }

        if (coords.x < 6)
        {
            if (coords.y != 7)
            {
                if (pieces[coords.x + 2, coords.y + 1] == null)
                {
                    possibleMoves[coords.x + 2, coords.y + 1] = true;
                }
            }
            if (coords.y != 0)
            {
                if (pieces[coords.x + 2, coords.y - 1] == null)
                {
                    possibleMoves[coords.x + 2, coords.y - 1] = true;
                }
            }
        }

        if (coords.x > 1)
        {
            if (coords.y != 7)
            {
                if (pieces[coords.x - 2, coords.y + 1] == null)
                {
                    possibleMoves[coords.x - 2, coords.y + 1] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 2, coords.y - 1] == null)
                {
                    possibleMoves[coords.x - 2, coords.y - 1] = true;
                }
            }
        }

        return possibleMoves;
    }
    public override bool[,] attacks(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleAttacks = new bool[8, 8];

        if (coords.y < 6)
        {
            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y + 2] != null)
                {
                    if (pieces[coords.x + 1, coords.y + 2].faction != fac)
                        possibleAttacks[coords.x + 1, coords.y + 2] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y + 2] != null)
                {
                    if (pieces[coords.x - 1, coords.y + 2].faction != fac)
                        possibleAttacks[coords.x - 1, coords.y + 2] = true;
                }
            }
        }

        if (coords.y > 1)
        {
            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y - 2] != null)
                {
                    if (pieces[coords.x + 1, coords.y - 2].faction != fac)
                        possibleAttacks[coords.x + 1, coords.y - 2] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y - 2] != null)
                {
                    if (pieces[coords.x - 1, coords.y - 2].faction != fac)
                        possibleAttacks[coords.x - 1, coords.y - 2] = true;
                }
            }
        }

        if (coords.x < 6)
        {
            if (coords.y != 7)
            {
                if (pieces[coords.x + 2, coords.y + 1] != null)
                {
                    if (pieces[coords.x + 2, coords.y + 1].faction != fac)
                        possibleAttacks[coords.x + 2, coords.y + 1] = true;
                }
            }
            if (coords.y != 0)
            {
                if (pieces[coords.x + 2, coords.y - 1] != null)
                {
                    if (pieces[coords.x + 2, coords.y - 2].faction != fac)
                        possibleAttacks[coords.x + 2, coords.y - 1] = true;
                }
            }
        }

        if (coords.x > 1)
        {
            if (coords.y != 7)
            {
                if (pieces[coords.x - 2, coords.y + 1] != null)
                {
                    if (pieces[coords.x - 2, coords.y + 1].faction != fac)
                        possibleAttacks[coords.x - 2, coords.y + 1] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 2, coords.y - 1] != null)
                {
                    if (pieces[coords.x - 2, coords.y - 1].faction != fac)
                        possibleAttacks[coords.x - 2, coords.y - 1] = true;
                }
            }
        }

        return possibleAttacks;
    }
}