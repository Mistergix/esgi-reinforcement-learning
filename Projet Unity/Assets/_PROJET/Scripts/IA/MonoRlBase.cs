using PGSauce.Unity;

namespace PGSauce.Games.IaEsgi.Ia
{
    public abstract class MonoRlBase : PGMonoBehaviour
    {
        public abstract AlgorithmBase AlgorithmBase { get; }
        public abstract void ResetGameForNewEpoch();
    }
}