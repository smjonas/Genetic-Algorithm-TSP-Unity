using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chromosome {

    public List<City> dna;
    public float fitness;

    public enum CrossoverType { ERX, OX }
    private CrossoverType crossoverType;

    private World world;
    public int generation;

    public Chromosome(World world) {

        this.world = world;
        this.crossoverType = world.crossoverType;
        this.dna = new List<City>(world.cities);
        dna.ShuffleList();
    }

    /// <summary>
    /// Calculates fitness as the inverse of the route length.
    /// </summary>
    public void CalculateFitness() {
        fitness = 1 / GetDistance();
    }

    /// <summary>
    /// Returns the total length of the route between all cities.
    /// </summary>
    public float GetDistance() {

        var distance = 0f;
        for (int i = 0; i < dna.Count; i++) {

            if (i == dna.Count - 1) {
                distance += dna[i].GetDistance(dna[0]);
            } else {
                distance += dna[i].GetDistance(dna[i+1]);
            }
        }
        return distance;
    }

    /// <summary>
    /// Combines the DNAs of two parents and returns a child using
    /// the method specified by <see cref="World.crossoverType"/>.
    /// </summary>    

    public Chromosome Crossover(Chromosome partner) {

        switch (crossoverType) {
            case CrossoverType.ERX:
                return CrossoverERX(partner);
            case CrossoverType.OX:
                return CrossoverOX(partner);
        }
        return null;
    }

    /// <summary>
    /// Combines the DNAs of two parents and returns a child
    /// (using Edge Recombination Crossover / ERX).
    /// </summary>    
    private Chromosome CrossoverERX(Chromosome partner) {

        var size = this.dna.Count;
        Chromosome child = new Chromosome(world);
        child.dna.Clear();

        var edges = new List<City>[world.cityCount];
        for (int i = 0; i < this.dna.Count; i++) {
            edges[i] = FindEdges(i, partner);
        }

        var currentCity = this.dna[0];
        while (child.dna.Count < this.dna.Count) {

            child.dna.Add(currentCity);

            for (int i = 0; i < edges.GetLength(0); i++) {
                edges[i].RemoveAll(x => x.index == currentCity.index);
            }

            if (edges[currentCity.index].Count == 0) {

                for (int i = 0; i < this.dna.Count; i++) {
                    if (!child.dna.Any(x => x.index == this.dna[i].index)) {
                        currentCity = this.dna[i];
                        break;
                    }
                }
            }
            else {

                var shortest = edges[currentCity.index][Random.Range(0, edges[currentCity.index].Count)];
                for (int j = 0; j < edges[currentCity.index].Count; j++) {

                    var edge = edges[currentCity.index][j];
                    if (edges[edge.index].Count < shortest.index) {
                        shortest = edge;
                    }
                }
                currentCity = shortest;
            }
        }

        return child;
    }

    /// <summary>
    /// Combines the DNAs of two parents and returns a child
    /// (using Ordered Crossover).
    /// </summary>
    private Chromosome CrossoverOX(Chromosome partner) {

        var size = this.dna.Count;
        Chromosome child = new Chromosome(world);
        child.dna.Clear();

        var startCut = Random.Range(0, size);
        var endCut = Random.Range(startCut + 1, size);
        var slice = this.dna.Skip(startCut).Take(endCut - startCut).ToList();

        for (int k = 0; k < size; k++) {
            if (!slice.Contains(partner.dna[k])) {
                slice.Add(partner.dna[k]);
            }
        }
        child.dna = slice;

        return child;
    }

    /// <summary>
    /// Returns a list of all neighboring cities of a specific city
    /// in the DNAs of both parents.
    /// </summary>
    private List<City> FindEdges(int city, Chromosome partner) {

        var cityEdges = new List<City>();
        for (int i = 0; i < 2; i++) {

            var parentDNA = i == 0 ? this.dna : partner.dna;
            for (int j = 0; j < parentDNA.Count; j++) {

                if ((j == 0 && (parentDNA[parentDNA.Count - 1].index == city || parentDNA[j + 1].index == city)) ||
                    (j == parentDNA.Count - 1 && (parentDNA[j - 1].index == city || parentDNA[0].index == city)) ||
                    (j > 0 && j < parentDNA.Count - 1 && (parentDNA[j - 1].index == city || parentDNA[j + 1].index == city))) {

                    if (!cityEdges.Contains(parentDNA[j]) && parentDNA[j].index != city) {
                        cityEdges.Add(parentDNA[j]);
                    }
                }
            }
        }
        return cityEdges;
    }

    /// <summary>
    /// Mutates each city in the DNA by swapping two random cities
    /// (probability is specified by the <paramref name="mutationRate"/>).
    /// </summary>
    public void Mutate(float mutationRate) {

        for (int i = 0; i < dna.Count; i++) {
            if (Random.value < mutationRate) {

                var city2Index = Random.Range(0, dna.Count);

                int numTries = world.itersToFindNewCity;
                // cities are the same
                while (city2Index == i && numTries > 0) {
                    city2Index = Random.Range(0, dna.Count);
                    numTries--;
                }

                // swap cities
                var temp = dna[i];
                dna[i] = dna[city2Index];
                dna[city2Index] = temp;
            }
        }
    }

    /// <summary>
    /// Returns a copy of the chromosome.
    /// </summary>
    public Chromosome Clone() {
        var clone = new Chromosome(world);
        clone.dna = this.dna;
        return clone;
    }

    /// <summary>
    /// Draws connection lines between cities in the DNA.
    /// </summary>
    public void Draw() {

        for (int i = 0; i < dna.Count; i++) {

            if (i == dna.Count - 1) {
                dna[i].DrawLine(dna[0]);
            }
            else dna[i].DrawLine(dna[i + 1]);
        }
    }

    public void SetGeneration(int generation) {
        this.generation = generation;
    }
}
