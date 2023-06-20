using CornerCheckers.Models;
using CornerCheckers.ViewModels.Base;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CornerCheckers.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        #region Поля и ссылки
        private Board board = new();
        private ICommand? newGameCommand;
        private ICommand? cellCommand;
        private CellValueEnum currentPlayer = CellValueEnum.WhiteChecker;
        #endregion

        #region Свойства
        public Board Board
        {
            get => board;
            set
            {
                board = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Команда запуска новой игры
        public ICommand NewGameCommand => newGameCommand ??= new Command(parameter =>
        {
            SetupBoard();
            currentPlayer = CellValueEnum.WhiteChecker;
        });
        #endregion

        #region Команда передвижения шашки
        public ICommand CellCommand => cellCommand ??= new Command(parameter =>
        {
            Cell cell = (Cell)parameter;
            Cell? activeCell = Board.FirstOrDefault(x => x.Act);

            if (cell.Cellvalueenum != CellValueEnum.Empty)
            {
                if (!cell.Act && (activeCell == null || cell == activeCell)) cell.Act = true;
                else cell.Act = false;
            }
            else if (activeCell != null && currentPlayer == CellValueEnum.WhiteChecker &&
                ((activeCell.Row - cell.Row == 1 && Math.Abs(activeCell.Column - cell.Column) == 1) ||
                (activeCell.Row - cell.Row == 2 && activeCell.Column == cell.Column &&
                Board.Any(x => x.Row == activeCell.Row - 1 && x.Column == activeCell.Column && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum)) ||
                (activeCell.Column - cell.Column == 2 && activeCell.Row == cell.Row &&
                Board.Any(x => x.Row == activeCell.Row && x.Column == activeCell.Column - 1 && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum)) ||
                (activeCell.Column - cell.Column == -2 && activeCell.Row == cell.Row &&
                Board.Any(x => x.Row == activeCell.Row && x.Column == activeCell.Column + 1 && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum))))
         {
                activeCell.Act = false;

                if (activeCell.Row - cell.Row == 2 && activeCell.Column == cell.Column)
                {
                    int capturedRow = activeCell.Row - 1;
                    int capturedColumn = activeCell.Column;
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }
                else if (activeCell.Row - cell.Row == -2 && activeCell.Column == cell.Column)
                {
                    int capturedRow = activeCell.Row + 1;
                    int capturedColumn = activeCell.Column;
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }
                else if (activeCell.Row == cell.Row && Math.Abs(activeCell.Column - cell.Column) == 2)
                {
                    int capturedRow = activeCell.Row;
                    int capturedColumn = activeCell.Column + (activeCell.Column - cell.Column > 0 ? -1 : 1);
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }

                cell.Cellvalueenum = activeCell.Cellvalueenum;
                activeCell.Cellvalueenum = CellValueEnum.Empty;

                if (Board.VictoryCondition(currentPlayer))
                {
                    if (currentPlayer == CellValueEnum.WhiteChecker)
                    {
                        ShowEndGameMessage(false);
                        SetupBoard();
                    }
                    else
                    {
                        ShowEndGameMessage(true);
                        SetupBoard();
                    }
                }
                else
                {
                    currentPlayer = currentPlayer == CellValueEnum.WhiteChecker ? CellValueEnum.BlackChecker : CellValueEnum.WhiteChecker;
                }
            }
            else if (activeCell != null && currentPlayer == CellValueEnum.BlackChecker &&
                ((activeCell.Row - cell.Row == -1 && Math.Abs(activeCell.Column - cell.Column) == 1) ||
                (activeCell.Column - cell.Column == 2 && activeCell.Row == cell.Row &&
                Board.Any(x => x.Row == activeCell.Row && x.Column == activeCell.Column - 1 && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum)) ||
                (activeCell.Column - cell.Column == -2 && activeCell.Row == cell.Row &&
                Board.Any(x => x.Row == activeCell.Row && x.Column == activeCell.Column + 1 && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum)) ||
                (activeCell.Row - cell.Row == -2 && activeCell.Column == cell.Column &&
                Board.Any(x => x.Row == activeCell.Row + 1 && x.Column == activeCell.Column && x.Cellvalueenum != CellValueEnum.Empty && x.Cellvalueenum != activeCell?.Cellvalueenum))))
            {
                activeCell.Act = false;

                if (activeCell.Row - cell.Row == -2 && activeCell.Column == cell.Column)
                {
                    int capturedRow = activeCell.Row + 1;
                    int capturedColumn = activeCell.Column;
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }
                else if (activeCell.Row - cell.Row == 1 && activeCell.Column == cell.Column)
                {
                    int capturedRow = activeCell.Row - 1;
                    int capturedColumn = activeCell.Column;
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }
                else if (activeCell.Row == cell.Row && Math.Abs(activeCell.Column - cell.Column) == 2)
                {
                    int capturedRow = activeCell.Row;
                    int capturedColumn = activeCell.Column + (activeCell.Column - cell.Column > 0 ? -1 : 1);
                    Cell? capturedCell = Board.FirstOrDefault(x => x.Row == capturedRow && x.Column == capturedColumn);

                    if (capturedCell != null && capturedCell.Cellvalueenum != CellValueEnum.Empty && capturedCell.Cellvalueenum != activeCell.Cellvalueenum)
                    {
                        capturedCell.Cellvalueenum = CellValueEnum.Empty;
                    }
                }

                cell.Cellvalueenum = activeCell.Cellvalueenum;
                activeCell.Cellvalueenum = CellValueEnum.Empty;

                if (Board.VictoryCondition(currentPlayer))
                {
                    if (currentPlayer == CellValueEnum.WhiteChecker)
                    {
                        ShowEndGameMessage(false);
                        SetupBoard();
                    }
                    else
                    {
                        ShowEndGameMessage(true);
                        SetupBoard();
                    }
                }
                else
                {
                    currentPlayer = currentPlayer == CellValueEnum.WhiteChecker ? CellValueEnum.BlackChecker : CellValueEnum.WhiteChecker;
                }
            }

        }, parameter => parameter is Cell cell && (Board.Any(x => x.Act) || cell.Cellvalueenum != CellValueEnum.Empty && cell.Cellvalueenum == currentPlayer));
        #endregion
        #region Дамки

        #endregion
        #region Стартовая доска
        private void SetupBoard()
        {
            Board board = new();
            board[0, 0] = CellValueEnum.BlackChecker;
            board[0, 1] = CellValueEnum.BlackChecker;
            board[0, 2] = CellValueEnum.BlackChecker;
            board[0, 3] = CellValueEnum.BlackChecker;
            board[0, 4] = CellValueEnum.BlackChecker;
            board[0, 5] = CellValueEnum.BlackChecker;
            board[0, 6] = CellValueEnum.BlackChecker;
            board[0, 7] = CellValueEnum.BlackChecker;
            board[1, 1] = CellValueEnum.BlackChecker;
            board[1, 0] = CellValueEnum.BlackChecker;
            board[1, 2] = CellValueEnum.BlackChecker;
            board[1, 3] = CellValueEnum.BlackChecker;
            board[1, 4] = CellValueEnum.BlackChecker;
            board[1, 5] = CellValueEnum.BlackChecker;
            board[1, 6] = CellValueEnum.BlackChecker;
            board[1, 7] = CellValueEnum.BlackChecker;
            board[6, 0] = CellValueEnum.WhiteChecker;
            board[6, 1] = CellValueEnum.WhiteChecker;
            board[6, 2] = CellValueEnum.WhiteChecker;
            board[6, 3] = CellValueEnum.WhiteChecker;
            board[6, 4] = CellValueEnum.WhiteChecker;
            board[6, 5] = CellValueEnum.WhiteChecker;
            board[6, 6] = CellValueEnum.WhiteChecker;
            board[6, 7] = CellValueEnum.WhiteChecker;
            board[7, 0] = CellValueEnum.WhiteChecker;
            board[7, 1] = CellValueEnum.WhiteChecker;
            board[7, 2] = CellValueEnum.WhiteChecker;
            board[7, 3] = CellValueEnum.WhiteChecker;
            board[7, 4] = CellValueEnum.WhiteChecker;
            board[7, 5] = CellValueEnum.WhiteChecker;
            board[7, 6] = CellValueEnum.WhiteChecker;
            board[7, 7] = CellValueEnum.WhiteChecker;
            Board = board;
        }
        #endregion

        #region Уведомление о победителе
        public static void ShowEndGameMessage(bool isWhiteWinner)
        {
            string winner = isWhiteWinner ? "Чёрные шашки" : "Белые шашки";
            MessageBox.Show($"Конец игры. Победитель - {winner}.", "Конец игры", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Конструктор
        public MainWindowViewModel() { SetupBoard(); }
        #endregion
    }
}
