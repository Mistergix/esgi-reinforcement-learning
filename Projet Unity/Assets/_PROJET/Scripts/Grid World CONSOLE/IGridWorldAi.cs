namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    public interface IGridWorldAi
    {
        public QStateGridWorldConsole GoUp();

        public QStateGridWorldConsole GoDown();

        public QStateGridWorldConsole GoLeft();
        public QStateGridWorldConsole GoRight();
        float GetTileValue(Coords coords);
    }
}