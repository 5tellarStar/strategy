using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;


public class PieceBase
{
    public string name = null;

    public int value;

    public int[,] placeValue = new int[8,8];

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

        value = 100;

        placeValue = new int[8, 8]{
            { 0, 50, 10, 5, 0, 5, 5, 0},
            { 0, 50, 10, 5, 0, -5, 10, 0},
            { 0, 50, 20, 10, 0, -10, 10, 0},
            { 0, 50, 30, 25, 20, 0, -20, 0},
            { 0, 50, 30, 25, 20, 0, -20, 0},
            { 0, 50, 20, 10, 0, -10, 10, 0},
            { 0, 50, 10, 5, 0, -5, 10, 0},
            { 0, 50, 10, 5, 0, 5, 5, 0},
        };
            

        description = "The weakest piece";

        sprite = 0;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.x != (fac == Faction.player ? 7 : 0))
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

        if (coords.x != (fac == Faction.player ? 7 : 0))
        {
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
        }
        return possibleAttacks;
    }
}

public class Rook : PieceBase
{
    public Rook()
    {
        name = "Rook";

        value = 500;

        placeValue = new int[8, 8]{
            { 0, 5, -5, -5, -5, -5, -5, 0},
            { 0, 10, 0, 0, 0, 0, 0, 0},
            { 0, 10, 0, 0, 0, 0, 0, 0},
            { 0, 10, 0, 0, 0, 0, 0, 5},
            { 0, 10, 0, 0, 0, 0, 0, 5},
            { 0, 10, 0, 0, 0, 0, 0, 0},
            { 0, 10, 0, 0, 0, 0, 0, 0},
            { 0, 5, -5, -5, -5, -5, -5, 0},
        };

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

        value = 320;

        placeValue = new int[8, 8]{
            { -50, -40, -30, -30, -30, -30, -40, -50},
            { -40, -20, 0, 5, 0, 5, -20, -40},
            { -30, 0, 10, 15, 15, 10, 0, -30},
            { -30, 0, 15, 20, 20, 15, 5, -30},
            { -30, 0, 15, 20, 20, 15, 5, -30},
            { -30, 0, 10, 15, 15, 10, 0, -30},
            { -40, -20, 0, 5, 0, 5, -20, -40},
            { -50, -40, -30, -30, -30, -30, -40, -50},
        };

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
            if (coords.y != 0)
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
                    if (pieces[coords.x + 2, coords.y - 1].faction != fac)
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
            if (coords.y != 0)
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

public class Bishop : PieceBase
{
    public Bishop()
    {
        name = "Bishop";

        value = 330;

        placeValue = new int[8, 8]{
            { -20, -10, -10, -10, -10, -10, -10, -20},
            { -10, 0, 0, 5, 0, 10, 5, -10},
            { -10, 0, 5, 5, 10, 10, 0, -10},
            { -10, 0, 10, 10, 10, 10, 0, -10},
            { -10, 0, 10, 10, 10, 10, 0, -10},
            { -10, 0, 5, 5, 10, 10, 0, -10},
            { -10, 0, 0, 5, 0, 10, 5, -10},
            { -20, -10, -10, -10, -10, -10, -10, -20},
        };

        description = "A piece that moves in an X shape";

        sprite = 3;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.x != 7)
        {
            if(coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i,coords.y + i] == null)
                    {
                        possibleMoves[coords.x + i, coords.y + i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y - i] == null)
                    {
                        possibleMoves[coords.x + i, coords.y - i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        if (coords.x != 0)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y + i] == null)
                    {
                        possibleMoves[coords.x - i, coords.y + i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y - i] == null)
                    {
                        possibleMoves[coords.x - i, coords.y - i] = true;
                    }
                    else
                    {
                        break;
                    }
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
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y + i] != null)
                    {
                        if(pieces[coords.x + i, coords.y + i].faction != fac)
                            possibleAttacks[coords.x + i, coords.y + i] = true;
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y - i] != null)
                    {
                        if (pieces[coords.x + i, coords.y - i].faction != fac)
                            possibleAttacks[coords.x + i, coords.y - i] = true;
                        break;
                    }
                }
            }
        }

        if (coords.x != 0)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y + i] != null)
                    {
                        if (pieces[coords.x - i, coords.y + i].faction != fac)
                            possibleAttacks[coords.x - i, coords.y + i] = true;
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y - i] != null)
                    {
                        if (pieces[coords.x - i, coords.y - i].faction != fac)
                            possibleAttacks[coords.x - i, coords.y - i] = true;
                        break;
                    }
                }
            }
        }

        return possibleAttacks;
    }
}

public class Queen : PieceBase
{
    public Queen()
    {
        name = "Queen";

        value = 900;

        placeValue = new int[8, 8]{
            { -20, -10, -10, -5, -5, -10, -10, -20},
            { -10, 0, 0, 0, 0, 0, 0, -10},
            { -10, 0, 5, 5, 5, 5, 0, -10},
            { -5, 0, 5, 5, 5, 5, 0, -5},
            { -5, 0, 5, 5, 5, 5, 0, -5},
            { -10, 0, 5, 5, 5, 5, 0, -10},
            { -10, 0, 0, 0, 0, 0, 0, -10},
            { -20, -10, -10, -5, -5, -10, -10, -20},
        };

        description = "The most powerful piece that combines the movement of a Rook and Bishop";

        sprite = 4;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.x != 7)
        {
            for (int x = coords.x + 1; x < 8; x++)
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


        if (coords.x != 7)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y + i] == null)
                    {
                        possibleMoves[coords.x + i, coords.y + i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y - i] == null)
                    {
                        possibleMoves[coords.x + i, coords.y - i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        if (coords.x != 0)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y + i] == null)
                    {
                        possibleMoves[coords.x - i, coords.y + i] = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y - i] == null)
                    {
                        possibleMoves[coords.x - i, coords.y - i] = true;
                    }
                    else
                    {
                        break;
                    }
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
                    if (pieces[x, coords.y].faction != fac)
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
                if (pieces[x, coords.y] != null)
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

        if (coords.x != 7)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y + i] != null)
                    {
                        if (pieces[coords.x + i, coords.y + i].faction != fac)
                            possibleAttacks[coords.x + i, coords.y + i] = true;
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && i + coords.x < 8; i++)
                {
                    if (pieces[coords.x + i, coords.y - i] != null)
                    {
                        if (pieces[coords.x + i, coords.y - i].faction != fac)
                            possibleAttacks[coords.x + i, coords.y - i] = true;
                        break;
                    }
                }
            }
        }

        if (coords.x != 0)
        {
            if (coords.y != 7)
            {
                for (int i = 1; i + coords.y < 8 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y + i] != null)
                    {
                        if (pieces[coords.x - i, coords.y + i].faction != fac)
                            possibleAttacks[coords.x - i, coords.y + i] = true;
                        break;
                    }
                }
            }
            if (coords.y != 0)
            {
                for (int i = 1; coords.y - i > -1 && coords.x - i > -1; i++)
                {
                    if (pieces[coords.x - i, coords.y - i] != null)
                    {
                        if (pieces[coords.x - i, coords.y - i].faction != fac)
                            possibleAttacks[coords.x - i, coords.y - i] = true;
                        break;
                    }
                }
            }
        }

        return possibleAttacks;
    }

}

public class King : PieceBase
{
    public King()
    {
        name = "King";

        value = 20000;

        placeValue = new int[8, 8]{
            { -30, -30, -30, -30, -20, -10, 20, 20},
            { -40, -40, -40, -40, -30, -20, 20, 30},
            { -40, -40, -40, -40, -30, -20, 0, 10},
            { -50, -50, -50, -50, -40, -20, 0, 0},
            { -50, -50, -50, -50, -40, -20, 0, 0},
            { -40, -40, -40, -40, -30, -20, 0, 10},
            { -40, -40, -40, -40, -30, -20, 20, 30},
            { -30, -30, -30, -30, -20, -10, 20, 20},
        };

        description = "The piece that needs to be protected at all cost";

        sprite = 5;
    }

    public override bool[,] moves(Piece[,] pieces, Vector2Int coords, Faction fac)
    {
        bool[,] possibleMoves = new bool[8, 8];

        if (coords.y != 7)
        {
            if (pieces[coords.x, coords.y + 1] == null)
            {
                possibleMoves[coords.x, coords.y + 1] = true;
            }

            if (coords.x != 7)
            {
                if (pieces[coords.x +1,coords.y + 1] == null)
                {
                    possibleMoves[coords.x +1,coords.y +1] = true;
                }
            }
            if(coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y + 1] == null)
                {
                    possibleMoves[coords.x - 1, coords.y + 1] = true;
                }
            }
        }

        if (coords.y != 0)
        {
            if (pieces[coords.x, coords.y - 1] == null)
            {
                possibleMoves[coords.x, coords.y - 1] = true;
            }

            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y - 1] == null)
                {
                    possibleMoves[coords.x + 1, coords.y - 1] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y - 1] == null)
                {
                    possibleMoves[coords.x - 1, coords.y - 1] = true;
                }
            }
        }

        if(coords.x != 7)
        {
            if (pieces[coords.x + 1, coords.y] == null)
            {
                possibleMoves[coords.x + 1, coords.y] = true;
            }
        }

        if (coords.x != 0)
        {
            if (pieces[coords.x - 1, coords.y] == null)
            {
                possibleMoves[coords.x - 1, coords.y] = true;
            }
        }

        if (coords.y == 4 && coords.x == (fac == Faction.player ? 0 : 7))
        {
            if (pieces[(fac == Faction.player ? 0 : 7),7] != null && pieces[(fac == Faction.player ? 0 : 7), 6] == null && pieces[(fac == Faction.player ? 0 : 7), 5] == null)
            {
                if(pieces[(fac == Faction.player ? 0 : 7), 7].faction == fac && pieces[(fac == Faction.player ? 0 : 7), 7].piece.name == "Rook")
                {
                    possibleMoves[(fac == Faction.player ? 0 : 7),6] = true;
                }
            }
            if (pieces[(fac == Faction.player ? 0 : 7), 0] != null && pieces[(fac == Faction.player ? 0 : 7), 1] == null && pieces[(fac == Faction.player ? 0 : 7), 2] == null && pieces[(fac == Faction.player ? 0 : 7), 3] == null)
            {
                if (pieces[(fac == Faction.player ? 0 : 7), 0].faction == fac && pieces[(fac == Faction.player ? 0 : 7), 0].piece.name == "Rook")
                {
                    possibleMoves[(fac == Faction.player ? 0 : 7), 2] = true;
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
            if (pieces[coords.x, coords.y + 1] != null)
            {
                if (pieces[coords.x,coords.y + 1].faction != fac)
                possibleAttacks[coords.x, coords.y + 1] = true;
            }

            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y + 1] != null)
                {
                    if (pieces[coords.x + 1, coords.y + 1].faction != fac)
                        possibleAttacks[coords.x + 1, coords.y + 1] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y + 1] != null)
                {
                    if (pieces[coords.x - 1, coords.y + 1].faction != fac)
                        possibleAttacks[coords.x - 1, coords.y + 1] = true;
                }
            }
        }

        if (coords.y != 0)
        {
            if (pieces[coords.x, coords.y - 1] != null)
            {
                if (pieces[coords.x, coords.y - 1].faction != fac)
                    possibleAttacks[coords.x, coords.y - 1] = true;
            }

            if (coords.x != 7)
            {
                if (pieces[coords.x + 1, coords.y - 1] != null)
                {
                    if (pieces[coords.x + 1, coords.y - 1].faction != fac)
                        possibleAttacks[coords.x + 1, coords.y - 1] = true;
                }
            }
            if (coords.x != 0)
            {
                if (pieces[coords.x - 1, coords.y - 1] != null)
                {
                    if (pieces[coords.x - 1, coords.y - 1].faction != fac)
                        possibleAttacks[coords.x - 1, coords.y - 1] = true;
                }
            }
        }

        if (coords.x != 7)
        {
            if (pieces[coords.x + 1, coords.y] != null)
            {
                if (pieces[coords.x + 1, coords.y].faction != fac)
                    possibleAttacks[coords.x + 1, coords.y] = true;
            }
        }

        if (coords.x != 0)
        {
            if (pieces[coords.x - 1, coords.y] != null)
            {
                if (pieces[coords.x - 1, coords.y].faction != fac)
                    possibleAttacks[coords.x - 1, coords.y] = true;
            }
        }

        return possibleAttacks;
    }
}
