using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CornerCheckers.Models
{
    public class Board : IEnumerable<Cell>
    {
        private readonly Cell[,] area;

        public CellValueEnum this[int row, int column]
        {
            get => area[row, column].Cellvalueenum;
            set => area[row, column].Cellvalueenum = value;
        }

        public Board()
        {
            area = new Cell[8, 8];
            for (int i = 0; i < area.GetLength(0); i++)
            {
                for (int j = 0; j < area.GetLength(1); j++)
                {
                    area[i, j] = new Cell(i, j, this);
                }
            }
        }

        public bool VictoryCondition(CellValueEnum currentPlayer)
        {
            int opponentCount = 0;

            foreach (var cell in area)
            {
                if (cell.Cellvalueenum != currentPlayer && cell.Cellvalueenum != CellValueEnum.Empty)
                {
                    opponentCount++;
                }
            }

            return opponentCount == 0;
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return area.Cast<Cell>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return area.GetEnumerator();
        }
    }
}