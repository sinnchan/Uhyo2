namespace Main.Domain.Entities.Game
{
    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        /// <summary>
        ///     現在の位置から指定した分移動します。
        /// </summary>
        /// <param name="direction">移動する方向</param>
        /// <param name="distance">移動する距離</param>
        /// <returns>移動後のPosition</returns>
        public Position MoveTo(Direction direction, int distance)
        {
            X += direction.GetDirectionValue().X * distance;
            Y += direction.GetDirectionValue().Y * distance;
            return this;
        }

        /// <summary>
        ///     同じ場所であるか確認します。
        /// </summary>
        public bool IsEqualTo(Position position)
        {
            return X == position.X && Y == position.Y;
        }

        public override string ToString()
        {
            return $"(X:{X} Y:{Y})";
        }
    }
}
