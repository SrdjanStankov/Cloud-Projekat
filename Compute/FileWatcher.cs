using System;
using System.IO;

namespace Compute
{
    public class FileWatcher
    {
        private bool canFireChangeEvent = true;
        private FileSystemWatcher watcher = new FileSystemWatcher { Filter = "*.dll" };

        public FileWatcher()
        {
            watcher.Changed += DllChanged;
            watcher.Created += DllCreated;
            watcher.Deleted += DllDeleted;
        }

        public void StartWatching()
        {
            watcher.Path = ComputeConfigurationContainer.ConfigLocation;
            watcher.EnableRaisingEvents = true;
        }

        // TODO: reaguj na promenu dll fajla

        /*
         * Nesto ovako npr.
         * AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => typeof(IDomainEntity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(x => x.Name).ToList();
         */

        private void DllDeleted(object sender, FileSystemEventArgs e) => Console.WriteLine("DLL has been removed");
        private void DllCreated(object sender, FileSystemEventArgs e) => Console.WriteLine("DLL has been created");

        private void DllChanged(object sender, FileSystemEventArgs e)
        {
            canFireChangeEvent = !canFireChangeEvent;
            if (!canFireChangeEvent)
            {
                return;
            }
            Console.WriteLine("DLL Has changed");
        }
    }
}
