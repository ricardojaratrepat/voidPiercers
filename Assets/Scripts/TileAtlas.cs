using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TileAtlas", menuName = "Tile Atlas")]
public class TileAtlas : ScriptableObject
{
    public TileClass dirt;
    public TileClass grass;
    public TileClass stone;
    public TileClass coal;
    public TileClass iron;
    public TileClass ice;  
}
