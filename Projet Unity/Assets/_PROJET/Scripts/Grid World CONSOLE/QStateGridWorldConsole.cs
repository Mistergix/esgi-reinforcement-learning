using PGSauce.Games.IaEsgi.Ia;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public class QStateGridWorldConsole : QState
    {
        private Coords _coords;
        public QStateGridWorldConsole(Coords coords)
        {
            _coords = coords;
        }

        public Coords Coords => _coords;
    }
}