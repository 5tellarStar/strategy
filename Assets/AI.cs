using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public struct Move
{
    public Piece piece;
    public Vector2Int target;
}

public class AIBase
{
    public virtual Move calculateMove(Piece[,] pieces)
    {
        return new Move();
    }

    static public List<Move> PossibleMoves(Piece[,] pieces, Faction fac)
    {
        List<Move> list = new();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] != null)
                {
                    if (pieces[x, y].faction == fac)
                    {
                        bool[,] possibleMoves = pieces[x, y].piece.moves(pieces, pieces[x, y].coordinates, fac);
                        bool[,] possibleAttacks = pieces[x, y].piece.attacks(pieces, pieces[x, y].coordinates, fac);

                        for (int bx = 0; bx < 8; bx++)
                        {
                            for (int by = 0; by < 8; by++)
                            {
                                if (possibleAttacks[bx, by] || possibleMoves[bx, by])
                                {
                                    Move move = new Move();
                                    move.piece = pieces[x, y];
                                    move.target = new Vector2Int(bx, by);
                                    list.Add(move);
                                }
                            }
                        }
                    }
                }
            }
        }
        return list;
    }

    static public Move BestMove(Piece[,] pieces, Faction fac)
    {
        List<Move> list = PossibleMoves(pieces, fac);

        Move bestMove = new Move();
        int bestScore = -1000000000;

        foreach (Move move in list)
        {
            Piece[,] resultingBoard = new Piece[8, 8];

            Array.Copy(pieces, resultingBoard, pieces.Length);

            if (move.piece.piece.name == "King" && move.piece.coordinates == new Vector2(move.piece.faction == Faction.player ? 0 : 7, 4))
            {
                if (move.target == new Vector2Int(move.piece.faction == Faction.player ? 0 : 7, 6))
                {
                    resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 5] = resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 7];
                    resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 7] = null;
                }
                if (move.target == new Vector2Int(move.piece.faction == Faction.player ? 0 : 7, 2))
                {
                    resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 3] = resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 0];
                    resultingBoard[move.piece.faction == Faction.player ? 0 : 7, 0] = null;
                }
            }

            resultingBoard[move.target.x, move.target.y] = move.piece;
            resultingBoard[move.piece.coordinates.x, move.piece.coordinates.y] = null;

            int score = BoardValue(resultingBoard, fac);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

        }
        return bestMove;
    }

    static public int BoardValue(Piece[,] pieces, Faction fac)
    {
        int score = 0;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] != null)
                {
                    score += (pieces[x, y].faction == fac ? 1 : -1) * (pieces[x, y].piece.value + pieces[x, y].piece.placeValue[pieces[x,y].faction == Faction.enemy ? x : 7 - x, y]);
                }
            }
        }
        return score;
    }
}

public class RandomMove : AIBase
{
    public override Move calculateMove(Piece[,] pieces)
    {
        List<Move> list = PossibleMoves(pieces, Faction.enemy);
        return list[UnityEngine.Random.Range(0,list.Count)];
    }
}

public class ShortSightedAI : AIBase
{
    public override Move calculateMove(Piece[,] pieces)
    {
        return BestMove(pieces,Faction.enemy);
    }
}

public class MiniMaxAI : AIBase
{
    int depth = 3;
    public override Move calculateMove(Piece[,] pieces)
    {
        Board startingBoard = new Board(pieces,0);

        Move bestMove = new Move();
        int bestScore = -1000000000;

        List<Board> boards = startingBoard.Branch(depth);

        for (int i = 1; i < boards.Count; ++i)
        {
            if(boards[i].BoardValue(Faction.enemy) > bestScore && boards[i].levelsDeep == depth)
            {
                bestScore = boards[i].BoardValue(Faction.enemy);
                bestMove = boards[i].moveRoot;
            }
        }

        bestMove.piece = pieces[bestMove.piece.coordinates.x, bestMove.piece.coordinates.y];
        return bestMove;
    }

    
}


public class Board
{
    public int levelsDeep;
    public Board(Piece[,] pieces, int levelsDeep)
    {
        this.pieces = new Piece[8, 8];
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] != null)
                {
                    this.pieces[x, y] = pieces[x, y];
                }
            }
        }

        this.levelsDeep = levelsDeep;
    }

    public List<Board> Branch(int depth)
    {
        List<Board> result = new List<Board>();
        List<Move> moves = AIBase.PossibleMoves(pieces,Faction.enemy);

        int currentValue = BoardValue(Faction.enemy);

        List<int> alphasPruned = new();

        for (int i = 0; i < moves.Count; i++)
        {
            branches.Add(new Board(pieces, levelsDeep + 1));

            if (moveRoot.piece == new Move().piece && moveRoot.target == new Move().target)
            {
                branches[i].moveRoot = moves[i];
            }
            else
            {
                branches[i].moveRoot = moveRoot;
            }

            if (moves[i].piece.piece.name == "King" && moves[i].piece.coordinates == new Vector2(moves[i].piece.faction == Faction.player ? 0 : 7, 4))
            {
                if (moves[i].target == new Vector2Int(moves[i].piece.faction == Faction.player ? 0 : 7, 6))
                {
                    branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 5] = branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 7];
                    branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 7] = null;
                }
                if (moves[i].target == new Vector2Int(moves[i].piece.faction == Faction.player ? 0 : 7, 2))
                {
                    branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 3] = branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 0];
                    branches[i].pieces[moves[i].piece.faction == Faction.player ? 0 : 7, 0] = null;
                }
            }

            branches[i].pieces[moves[i].target.x, moves[i].target.y] = moves[i].piece;
            branches[i].pieces[moves[i].piece.coordinates.x, moves[i].piece.coordinates.y] = null;


            Move playerMove = AIBase.BestMove(branches[i].pieces, Faction.player);

            if (playerMove.piece.piece.name == "King" && playerMove.piece.coordinates == new Vector2(playerMove.piece.faction == Faction.player ? 0 : 7, 4))
            {
                if (playerMove.target == new Vector2Int(playerMove.piece.faction == Faction.player ? 0 : 7, 6))
                {
                    branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 5] = branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 7];
                    branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 7] = null;
                }
                if (playerMove.target == new Vector2Int(playerMove.piece.faction == Faction.player ? 0 : 7, 2))
                {
                    branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 3] = branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 0];
                    branches[i].pieces[playerMove.piece.faction == Faction.player ? 0 : 7, 0] = null;
                }
            }

            branches[i].pieces[playerMove.target.x, playerMove.target.y] = playerMove.piece;
            branches[i].pieces[playerMove.piece.coordinates.x, playerMove.piece.coordinates.y] = null;

            if (currentValue > branches[i].BoardValue(Faction.enemy))
            {
                alphasPruned.Add(i);
            }

        }
        if (alphasPruned.Count != branches.Count)
        {
            for (int i = alphasPruned.Count - 1; i >= 0;i--)
            {
                branches.RemoveAt(alphasPruned[i]);
            }
        }

        result.AddRange(branches);

        for (int i = 0;i < branches.Count;i++)
        {
            if (levelsDeep < depth)
                result.AddRange(branches[i].Branch(depth));
        }
        
        return result;
    }

    public Move moveRoot = new Move();

    public Piece[,] pieces;

    public List<Board> branches = new();

    public int BoardValue(Faction fac)
    {
        int score = 0;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] != null)
                {
                    score += (pieces[x, y].faction == fac ? 1 : -1) * (pieces[x, y].piece.value + pieces[x, y].piece.placeValue[pieces[x, y].faction == Faction.enemy ? x : 7 - x, y]);
                }
            }
        }
        return score;
    }
}