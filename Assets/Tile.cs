using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coords;
    public SpriteRenderer spriteRenderer;
    public BattleManager battleManager;
    private void OnMouseDown()
    {
        battleManager.ClickedTile(this);
    }
}
