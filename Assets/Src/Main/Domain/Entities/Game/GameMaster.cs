using System.Collections.Generic;
using System.Linq;

namespace Src.Main.Domain.Entities.Game
{
    public class GameMaster
    {
        private readonly Board _board;
        private PieceColor _nowTurn;

        public GameMaster()
        {
            _board = new Board();
            _nowTurn = PieceColor.Black;
            GameInit();
            GameEndFlag = false;
        }

        /// <summary>
        ///     テスト用コンストラクタ
        ///     もしかしたらロード機能で使うかも
        /// </summary>
        /// <param name="board"></param>
        public GameMaster(Board board)
        {
            _board = board;
            if (_board.IsEmpty())
            {
                GameInit();
                GameEndFlag = false;
                return;
            }

            // コマの数が偶数なら黒のターン。奇数なら代のターン。
            _nowTurn = _board.CountPiece() % 2 == 0 ? PieceColor.Black : PieceColor.White;
            GameEndFlag = board.IsFull(); //todo game終了判定が必要
        }

        public bool GameEndFlag { get; }

        /// <summary>
        ///     盤面初期化処理
        /// </summary>
        private void GameInit()
        {
            _board.Clear();
            _board.PlacePiece(Piece.CreateWhite(), new Position(4, 4));
            _board.PlacePiece(Piece.CreateBlack(), new Position(5, 4));
            _board.PlacePiece(Piece.CreateBlack(), new Position(4, 5));
            _board.PlacePiece(Piece.CreateWhite(), new Position(5, 5));
        }

        public List<Position> GetSuggestPositions()
        {
            foreach (var piece in _board.Data.Cast<Piece>())
                if (piece == null)
                {
                    //todo wip
                }

            return null;
        }
    }
}
