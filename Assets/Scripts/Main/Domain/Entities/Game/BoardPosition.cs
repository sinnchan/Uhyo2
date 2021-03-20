using System;
using NLog;

namespace Main.Domain.Entities.Game
{
    /// <summary>
    ///     オセロの盤面を想定したPosition指定クラス。
    ///     0 ~ 7しか代入できない仕組みです。
    /// </summary>
    public class BoardPosition : Position
    {
        public const int Max = Board.Length - 1;
        public const int Min = 0;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public BoardPosition(int x, int y) : base(TrimArg(x), TrimArg(y))
        {
            if (CheckArg(x))
                Logger.Warn("x is out of range. -> {0}", x);
            if (CheckArg(y))
                Logger.Warn("y is out of range. -> {0}", y);
        }

        /// <summary>
        ///     適切な値か確かめます。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool CheckArg(int value)
        {
            return Min <= value && value <= Max;
        }

        /// <summary>
        ///     適切な値にトリミングします。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int TrimArg(int value)
        {
            return Math.Max(Math.Min(value, Max), Min);
        }

        /// <summary>
        ///     指定した方向と距離の位置を返します。
        ///     盤面の範囲を超える場合は変更前の値を返します。
        /// </summary>
        /// <param name="direction">移動する方向</param>
        /// <param name="distance">移動する距離</param>
        /// <returns>成功判定つきの結果</returns>
        public new Result<BoardPosition> MoveTo(Direction direction, int distance)
        {
            var x = X + direction.GetDirectionValue().X * distance;
            var y = Y + direction.GetDirectionValue().Y * distance;

            if (CheckArg(x) && CheckArg(y))
                return new Result<BoardPosition>(new BoardPosition(x, y), true);

            Logger.Info("Failed to move to the specified position.");
            return new Result<BoardPosition>(this, false);
        }
    }
}
