using UnityEngine;

public class GlobalVariables
{
    public static int Lives { get; set; } = 3;
    public static int SpawnArea { get; set; } = 2;

    public static PieceBase[,] StartingPieces { get; set; } = new PieceBase[8, 8]; 
}
