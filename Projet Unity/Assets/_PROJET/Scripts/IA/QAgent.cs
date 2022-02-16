namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class QAgent
    {
        private QState _currentState;

        protected QAgent(QState currentState)
        {
            _currentState = currentState;
        }

        public QState CurrentState => _currentState;

        public void TakeAction(QAction action)
        {
            
        }
    }
}