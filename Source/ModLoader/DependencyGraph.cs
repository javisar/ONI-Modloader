namespace ModLoader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class DependencyGraph
    {
        private readonly Dictionary<string, Vertex> nameLookup;

        private readonly Vertex[] vertices;

        public DependencyGraph(List<Assembly> modAssemblies)
        {
            int size = modAssemblies.Count;
            this.vertices   = new Vertex[size];
            this.nameLookup = new Dictionary<string, Vertex>(size);

            // Create vertices
            for (int i = 0; i < size; ++i)
            {
                Assembly modAssembly = modAssemblies[i];
                string   modName     = Path.GetFileNameWithoutExtension(modAssembly.Location);

                Vertex modVertex = new Vertex(i, modAssembly, modName);
                this.vertices[i]                      = modVertex;
                this.nameLookup[modAssembly.FullName] = modVertex;
            }

            // Add edges
            foreach (Vertex modVertex in this.vertices)
            {
                Assembly modAssembly = modVertex.assembly;

                foreach (AssemblyName referenced in modAssembly.GetReferencedAssemblies())
                {
					//if (this.nameLookup.TryGetValue(referenced.FullName, out Vertex dependencyVertex))
					Vertex dependencyVertex;
					if (this.nameLookup.TryGetValue(referenced.FullName, out dependencyVertex))
					{
                        modVertex.dependencies.Add(dependencyVertex);
                        dependencyVertex.dependents.Add(modVertex);
                    }
                    else if (referenced.CodeBase != null)
                    {
                        ModLogger.WriteLine(
                                              "Dependency " + referenced.FullName + " at " + referenced.CodeBase
                                            + " was not a mod");
                    }
                }
            }
        }

        public List<Assembly> TopologicalSort()
        {
            int[]                      unloadedDependencies = new int[this.vertices.Length];
            SortedList<string, Vertex> loadableMods         = new SortedList<string, Vertex>();
            List<Assembly> loadedMods           = new List<Assembly>(this.vertices.Length);

            // Initialize the directory
            for (int i = 0; i < this.vertices.Length; ++i)
            {
                Vertex vertex          = this.vertices[i];
                int    dependencyCount = vertex.dependencies.Count;

                unloadedDependencies[i] = dependencyCount;
                if (dependencyCount == 0)
                {
                    loadableMods.Add(vertex.name, vertex);
                }
            }

            // Perform the (reverse) topological sorting
            while (loadableMods.Count > 0)
            {
                Vertex mod = loadableMods.Values[0];
                loadableMods.RemoveAt(0);
                loadedMods.Add(mod.assembly);

                foreach (Vertex dependent in mod.dependents)
                {
                    unloadedDependencies[dependent.index] -= 1;
                    if (unloadedDependencies[dependent.index] == 0)
                    {
                        loadableMods.Add(dependent.name, dependent);
                    }
                }
            }

            if (loadedMods.Count < this.vertices.Length)
            {
                throw new ArgumentException("Could not sort dependencies topologically due to a cyclic dependency.");
            }

            return loadedMods;
        }

        private class Vertex
        {
            internal readonly Assembly assembly;

            internal readonly List<Vertex> dependencies;

            internal readonly List<Vertex> dependents;

            internal readonly int index;

            internal readonly string name;

            internal Vertex(int index, Assembly assembly, string name)
            {
                this.index    = index;
                this.assembly = assembly;
                this.name     = name;

                this.dependencies = new List<Vertex>();
                this.dependents   = new List<Vertex>();
            }
        }
    }
}