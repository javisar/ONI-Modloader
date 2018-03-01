using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ModLoader
{

    public class DependencyGraph
    {

        private readonly Vertex[] vertices;
        private readonly Dictionary<string, Vertex> nameLookup;

        public DependencyGraph(List<Assembly> modAssemblies)
        {
            int size = modAssemblies.Count;
            vertices = new Vertex[size];
            nameLookup = new Dictionary<string, Vertex>(size);

            // Create vertices
            for (int i = 0; i < size; ++i)
            {
                Assembly modAssembly = modAssemblies[i];
                string modName = Path.GetFileNameWithoutExtension(modAssembly.Location);

                Vertex modVertex = new Vertex(i, modAssembly, modName);
                vertices[i] = modVertex;
                nameLookup[modAssembly.FullName] = modVertex;
            }

            // Add edges
            foreach (Vertex modVertex in vertices)
            {
                Assembly modAssembly = modVertex.assembly;

                foreach (AssemblyName referenced in modAssembly.GetReferencedAssemblies())
                {
                    Vertex dependencyVertex;
                    if (nameLookup.TryGetValue(referenced.FullName, out dependencyVertex))
                    {
                        modVertex.dependencies.Add(dependencyVertex);
                        dependencyVertex.dependents.Add(modVertex);
                    }
                    else if (referenced.CodeBase != null)
                    {
                        Debug.Log("Dependency " + referenced.FullName + " at " + referenced.CodeBase + " was not a mod");
                    }
                }
            }
        }

        public List<Assembly> TopologicalSort()
        {
            int[] unloadedDependencies = new int[vertices.Length];
            SortedList<String, Vertex> loadableMods = new SortedList<string, Vertex>();
            List<Assembly> loadedMods = new List<Assembly>(vertices.Length);

            // Initialize the directory
            for (int i = 0; i < vertices.Length; ++i)
            {
                Vertex vertex = vertices[i];
                int dependencyCount = vertex.dependencies.Count;

                unloadedDependencies[i] = dependencyCount;
                if (dependencyCount == 0)
                    loadableMods.Add(vertex.name, vertex);
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

            if (loadedMods.Count < vertices.Length)
                throw new ArgumentException("Could not sort dependencies topologically due to a cyclic dependency.");
            return loadedMods;
        }

        private class Vertex
        {

            internal readonly int index;
            internal readonly Assembly assembly;
            internal readonly string name;

            internal readonly List<Vertex> dependencies;
            internal readonly List<Vertex> dependents;

            internal Vertex(int index, Assembly assembly, string name)
            {
                this.index = index;
                this.assembly = assembly;
                this.name = name;

                dependencies = new List<Vertex>();
                dependents = new List<Vertex>();
            }
        }
    }
}