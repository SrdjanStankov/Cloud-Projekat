using Common;

namespace Compute
{
    public class ComputeMenagment : IComputeManagement
    {
        public string Scale(string assemblyName, int count)
        {
            if (count > 4)
            {
                return "Nepodrzana akcija.";
            }
            // TODO: Nepromenjen broj instanci
            else if (count == ContainerFactory.Instance.Containers.Count)
            {
                return "Nema promena";
            }
            else
            {
                int counter = 0;
                // TODO: Startuj / Stopiraj instance
                while (count > ContainerFactory.Instance.Containers.Count)
                {
                    counter++;
                    ContainerFactory.Instance.CreateAndStartContainer();
                }

                while (count < ContainerFactory.Instance.Containers.Count)
                {
                    counter--;
                    ContainerFactory.Instance.KillContainer();
                }

                return (counter > 0) ? $"Angazovano {counter}. novih instanci" : $"Stopirano {counter}. instanci";
            }
        }
    }
}
