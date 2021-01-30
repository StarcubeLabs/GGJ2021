namespace GGJ2021
{

    /// <summary>
    /// Used to provide custom initialization during tilemap initialization.
    /// </summary>
    interface TilemapInit
    {

        void InitFromTilemap(Tilemap3D tilemap);
    }
}