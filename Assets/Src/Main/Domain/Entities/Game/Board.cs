using System;
using System.Linq;
using JetBrains.Annotations;

namespace Src.Main.Domain.Entities.Game
{
    public class Board
    {
        public Board()
        {
            Data = new Piece[8, 8];
        }

        /// <summary>
        ///     テスト用コンストラクタ。
        ///     もしかしたらロード機能追加で使うかも
        /// </summary>
        /// <param name="data"></param>
        public Board(Piece[,] data)
        {
            Data = data;
        }

        [ItemCanBeNull] public Piece[,] Data { get; }

        /// <summary>
        ///     盤面すべてループして渡したActionを実行します。
        /// </summary>
        /// <param name="action">x, y</param>
        public static void LoopAccessAll(Action<int, int> action)
        {
            for (var i = 0; i < Math.Pow(Position.Max, 2); i++) action(i % Position.Max, i / Position.Max);
        }

        /// <summary>
        ///     すべてを無に帰します。
        /// </summary>
        public void Clear()
        {
            LoopAccessAll((x, y) => { Data[y, x] = null; });
        }

        /// <summary>
        ///     コマが存在しないならtrueを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Data.Cast<Piece>().All(piece => piece == null);
        }

        /// <summary>
        ///     盤面すべて埋まっているならtrueを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return Data.Cast<Piece>().All(piece => piece != null);
        }

        /// <summary>
        ///     指定した場所にコマを置きます。
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="position"></param>
        public void PlacePiece(Piece piece, Position position)
        {
            Data[position.Y, position.X] = piece;
        }

        /// <summary>
        ///     盤面に存在するコマを数えます。
        /// </summary>
        /// <returns></returns>
        public int CountPiece()
        {
            return Data.Cast<Piece>().Count(piece => piece != null);
        }
    }
}
