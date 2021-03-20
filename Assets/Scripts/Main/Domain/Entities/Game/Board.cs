using System;
using System.Linq;

namespace Main.Domain.Entities.Game
{
    public class Board
    {
        public const int Length = 8;
        public const int Max = Length * Length;
        private readonly Piece[,] _data;

        public Board()
        {
            _data = new Piece[Length, Length];
            LoopAccessAll(p => { _data[p.Y, p.X] = new Piece(PieceState.Space); });
        }

        /// <summary>
        ///     テスト用コンストラクタ。
        ///     もしかしたらロード機能追加で使うかも
        /// </summary>
        /// <param name="data"></param>
        public Board(Piece[,] data)
        {
            _data = data;
        }

        /// <summary>
        ///     ボードのコピーを返します。
        /// </summary>
        /// <returns></returns>
        public Board CreateCopy()
        {
            return new Board(_data);
        }

        /// <summary>
        ///     盤面すべてループして渡したActionを実行します。
        /// </summary>
        /// <param name="action">x, y</param>
        public static void LoopAccessAll(Action<BoardPosition> action)
        {
            for (var i = 0; i < Max; i++) action(new BoardPosition(i % Length, i / Length));
        }

        public Piece GetPiece(BoardPosition position)
        {
            return _data[position.Y, position.X];
        }

        /// <summary>
        ///     すべてを無に帰します。
        /// </summary>
        public void Clear()
        {
            LoopAccessAll(p => _data[p.Y, p.X].Clear());
        }

        /// <summary>
        ///     コマが存在しないならtrueを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _data.Cast<Piece>().All(piece => piece.State == PieceState.Space);
        }

        /// <summary>
        ///     盤面すべて埋まっているならtrueを返します。
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return _data.Cast<Piece>().All(piece => piece.State != PieceState.Space);
        }

        /// <summary>
        ///     指定した場所にコマを置きます。
        /// </summary>
        /// <param name="piece">コマ</param>
        /// <param name="position">1 ~ 8を入れる仕様</param>
        public void PlacePiece(Piece piece, BoardPosition position)
        {
            _data[position.Y, position.X] = piece;
        }

        /// <summary>
        ///     指定のコマを数えます。
        /// </summary>
        /// <param name="pieceState">カウントするコマ</param>
        /// <returns>カウント結果</returns>
        public int Count(PieceState pieceState)
        {
            return _data.Cast<Piece>().Count(piece => piece.State == pieceState);
        }

        public string VisualizeData()
        {
            var cache = "";
            var result = "";
            LoopAccessAll(p =>
            {
                cache += _data[p.Y, p.X].ToString();
                if (p.X != Length - 1) return;
                result += cache + Environment.NewLine;
                cache = "";
            });
            return result;
        }
    }
}
