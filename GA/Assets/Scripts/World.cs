using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class World : MonoBehaviour {

    public List<City> cities = new List<City>();

    [Tooltip("The number of cities in a route")]
    public int cityCount;

    public List<Chromosome> population = new List<Chromosome>();

    [Tooltip("The number of individuals in the population")]
    public int populationSize;

    
    [Tooltip("The number of best chromosomes which are transferred" +
        " to the next generation without mutating their DNA")]
    public int elitism;

    [Tooltip("The probability of each city in the DNA of a chromosome to be mutated")]
    public float mutationRate;

    [Tooltip("When true, chromosomes chosen as elites are mutated")]
    public bool mutateElites;

    [Tooltip("The probability of each city in the DNA of an elite chromosome to be mutated")]
    public float eliteMutationRate;

    [Tooltip("The maximum number of iterations to find a new random city during mutation" +
        " in case two identical cities are chosen")]
    public int itersToFindCities;

    private int generation = 1;
    private float fitnessSum;
    private Chromosome best, allTimeBest;

    [Tooltip("The selected method of crossing over two chromosomes")]
    public Chromosome.CrossoverType crossoverType;

    public GameObject cityPrefab;
    private List<City> newCities = new List<City>();

    public TextMeshProUGUI currentDist, currentGen, bestDist, bestGen;
    private bool toggled;
    private float startDist;

    /// <summary>
    /// Creates the initial population.
    /// </summary>
    void Start() {

        // create n cities at random locations
        for (int i = 0; i < cityCount; i++) {

            var city = Instantiate(cityPrefab, transform).GetComponent<City>();
            city.index = i;
            city.SetPosition(Random.Range(0f, 10f), Random.Range(0f, 10f));

            cities.Add(city);
        }

        // create new population
        for (int j = 0; j < populationSize; j++) {
            population.Add(new Chromosome(this));
        }
    }

    /// <summary>
    /// Evaluates the population and performs crossover (one generation).
    /// </summary>
    void Update() {

        #region Calculate fitness
        fitnessSum = 0f;
        for (int i = 0; i < population.Count; i++) {

            population[i].CalculateFitness();
            fitnessSum += population[i].fitness;
        }
        #endregion

        #region Selection
        // sort elements
        population = population.OrderByDescending(x => x.fitness).ToList();
        best = population[0];

        // draw best element of generation
        best.Draw();

        // add elements to matingPool according to their fitness
        var matingPool = new List<Chromosome>();
        for (int j = 0; j < population.Count; j++) {
            matingPool.Add(Select());
        }

        var newPopulation = new List<Chromosome>();
        while (newPopulation.Count < population.Count - 1 - elitism) { 

            // select two parents
            var parentA = matingPool[Random.Range(0, matingPool.Count)];
            var parentB = matingPool[Random.Range(0, matingPool.Count)];

            #region Crossover
            var child1 = parentA.Crossover(parentB);
            var child2 = parentA.Crossover(parentB);
            #endregion

            #region Mutation
            child1.Mutate(mutationRate);
            child2.Mutate(mutationRate);
            #endregion

            newPopulation.Add(child1);
            newPopulation.Add(child2);

            if (population.Count - newPopulation.Count - elitism == 1) {
                var child = parentA.Crossover(parentB);
                child.Mutate(mutationRate);
                newPopulation.Add(child);
            }
        }

        #region Elitism
        for (int i = 0; i < elitism; i++) {
            var dna = new List<City>();
            for (int j = 0; j < population[i].dna.Count; j++) {
                dna.Add(cities.Where(x => x.index == population[i].dna[j].index).FirstOrDefault());
            }
            
            var elite = new Chromosome(this);
            elite.dna = dna;
            if (mutateElites)
                elite.Mutate(eliteMutationRate);
            newPopulation.Add(elite);
        }
        #endregion

        population = newPopulation;
        generation++;
        #endregion

        #region Visualization
        // update text
        var distA = System.Math.Truncate(best.GetDistance() * 1000) / 1000;
        currentDist.text = "Distance: " + distA;
        currentGen.text = "Generation: " + generation;

        // draw best route
        if (allTimeBest == null || best.fitness > allTimeBest.fitness) {
            allTimeBest = best;
            allTimeBest.SetGeneration(generation);
            DrawBest();

            ShowChange();
            bestGen.text = "Generation: " + allTimeBest.generation;
        }

        if (generation == 2)
            startDist = best.GetDistance();
        #endregion
    }

    /// <summary>
    /// Returns a chromosome chosen proportionally to its fitness
    /// using roulette wheel selection. 
    /// </summary>
    private Chromosome Select() {

        float num = Random.Range(0f, fitnessSum);
        float sum = 0f;

        for (int i = 0; i < population.Count; i++) {
            sum += population[i].fitness;
            if (sum > num) {
                return population[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Draws all-time best route. 
    /// </summary>
    private void DrawBest() {

        for (int i = 0; i < newCities.Count; i++) {
            Destroy(newCities[i].gameObject);
        }

        newCities.Clear();
        for (int i = 0; i < allTimeBest.dna.Count; i++) {

            var city = Instantiate(cityPrefab, transform).GetComponent<City>();
            city.index = allTimeBest.dna[i].index;
            city.transform.position = new Vector3(allTimeBest.dna[i].transform.position.x + 13,
                                                  allTimeBest.dna[i].transform.position.y);
            newCities.Add(city);
        }
        allTimeBest.dna = newCities.ToList();
        allTimeBest.Draw();
    }

    /// <summary>
    /// Displays relative distance difference of the current best route 
    /// to the starting route (can be toggled).
    /// </summary>
    public void ShowChange() {

        var distB = System.Math.Truncate(allTimeBest.GetDistance() * 1000) / 1000;
        if (toggled) {
            var change = (allTimeBest.GetDistance() - startDist) / startDist * 100f;
            bestDist.text = "Distance: " + distB + "   (" + System.Math.Truncate(change * 100f) / 100f + "%)";
        } else {
            bestDist.text = "Distance: " + distB;
        }
    }

    /// <summary>
    /// Shows or hides relative distance difference.
    /// See method <see cref="ShowChange()"/>.
    /// </summary>
    public void TogglePercentage() {
        toggled = !toggled;
        ShowChange();
    }
}
