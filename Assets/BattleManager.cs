using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private CursorController cursor;

    public bool Playerturn;

    public Piece[,] pieces = new Piece[8, 8];
    public Tile[,] tiles = new Tile[8, 8];

    public Piece selectedPiece;

    public List<Tile> possibleMoves;
    public List<Piece> possibleAttacks;
    private void Start()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x - 3.5f, 3.8125f - y * 0.75f, 0);
                tile.GetComponent<Tile>().battleManager = this;
                tile.GetComponent<Tile>().coords = new Vector2Int(x, y);
                tiles[x, y] = tile.GetComponent<Tile>();
            }
        }

        AddPiece(new Vector2Int(0, 0), Faction.player, new Pawn());
        AddPiece(new Vector2Int(0, 1), Faction.player, new Pawn());
        AddPiece(new Vector2Int(0, 2), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 0), Faction.enemy, new Pawn());

        AddPiece(new Vector2Int(4,5), Faction.player, new King());
    }
    private void AddPiece(Vector2Int coords, Faction fac, PieceBase p)
    {
        Piece piece = Instantiate(piecePrefab).GetComponent<Piece>();
        piece.New(coords, fac, p, this);
        pieces[coords.x, coords.y] = piece;
    }

    public void ClickedPiece(Piece piece)
    {
        if (Playerturn && selectedPiece == null && piece.faction == Faction.player)
        {
            selectedPiece = piece;
            cursor.Grab(piece);
            piece.Grab();

            bool[,] possibleM = piece.piece.moves(pieces, piece.coordinates, Faction.player);
            bool[,] possibleA = piece.piece.attacks(pieces, piece.coordinates, Faction.player);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (possibleM[x, y])
                    {
                        possibleMoves.Add(tiles[x, y]);
                    }
                    if (possibleA[x, y])
                    {
                        possibleAttacks.Add(pieces[x, y]);
                    }
                }
            }
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                possibleMoves[i].spriteRenderer.color = Color.yellow;
            }
            for (int i = 0; i < possibleAttacks.Count; i++)
            {
                possibleAttacks[i].spriteRenderer.color = Color.red;
            }
        }
        else if(selectedPiece != null)
        {
            bool possible = false;
            foreach (Piece p in possibleAttacks)
            {
                if(p == piece)
                {
                    possible = true;
                }
            }

            if (possible)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (pieces[x, y] == selectedPiece)
                        {
                            pieces[x, y] = null;
                        }
                    }
                }
                pieces[piece.coordinates.x, piece.coordinates.y] = selectedPiece;
                GameObject ragdoll = Instantiate(ragdollPrefab);
                ragdoll.transform.position = piece.transform.position;
                ragdoll.GetComponent<SpriteRenderer>().sprite = piece.faction == Faction.player ? GetComponent<SpriteLib>().spritesPSmall[piece.piece.sprite] : GetComponent<SpriteLib>().spritesESmall[piece.piece.sprite];
                Destroy(piece.gameObject);
                selectedPiece.Place(piece.coordinates);
                cursor.LetGo();
            }
            else
            {
                selectedPiece.Return();
                cursor.LetGo();
            }

            selectedPiece = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y].spriteRenderer.color = Color.clear;
                    if (pieces[x, y] != null)
                        pieces[x, y].spriteRenderer.color = Color.white;
                }
            }
            possibleMoves = new();
            possibleAttacks = new();
        }

    }

    public void ClickedTile(Tile tile)
    {
        if (selectedPiece != null)
        {
            bool possible = false;
            foreach (Tile possibles in possibleMoves)
            {
                if (tile == possibles)
                {
                    possible = true;
                }
            }
            if (possible)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (pieces[x, y] == selectedPiece)
                        {
                            pieces[x, y] = null;
                        }
                    }
                }
                pieces[tile.coords.x, tile.coords.y] = selectedPiece;
                selectedPiece.Place(tile.coords);
                cursor.LetGo();
            }
            else
            {
                selectedPiece.Return();
                cursor.LetGo();
            }
            selectedPiece = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y].spriteRenderer.color = Color.clear;
                    if (pieces[x, y] != null)
                        pieces[x, y].spriteRenderer.color = Color.white;
                }
            }
            possibleMoves = new();
            possibleAttacks = new();
        }
    }
}

public enum Faction
{
    player,
    enemy
}