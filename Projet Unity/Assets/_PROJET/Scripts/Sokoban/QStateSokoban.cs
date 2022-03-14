using System.Collections.Generic;
using System.Text;
using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.Sokoban
{
    public class QStateSokoban : QState
    {
        private Coords _playerCoords;
        private List<Coords> _cratesCoords;

        public QStateSokoban(Coords playerCoords, List<Coords> cratesCoords)
        {
            _playerCoords = playerCoords;
            _cratesCoords = cratesCoords;
        }

        public Coords PlayerCoords => _playerCoords;

        public List<Coords> CratesCoords => _cratesCoords;

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var coord in _cratesCoords)
            {
                sb.Append(coord.ToString());
                sb.Append(";");
            }
            
            return $"[STATE SOKOBAN {_playerCoords} ({sb})]";
        }
    }
}
