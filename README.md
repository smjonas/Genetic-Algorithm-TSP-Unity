# Genetic Algorithm to solve the TSP in Unity

![Image](https://github.com/jonasstr/Genetic-Algorithm-TSP-Unity/blob/master/images/tsp.PNG)

## About
This Unity project can be used to visualize solving the Travelling Salesman Problem (TSP) using an implementation of the Genetic Algorithm.

## Installation
Clone or download the project and open it in Unity (File &rarr; Open Project).

## Parameters
The following variables can be adjusted in the Unity inspector by clicking on the World object in the hierarchy.

- `cityCount` - The number of cities in a route (default: 20)  
- `populationSize` - The number of individuals in the population (default: 18)  
- `mutationRate` - The probability of each city in the DNA of a chromosome to be mutated (default: 0.013)  
- `elitism` - The number of best chromosomes which are transferred to the next generation (default: 6)  
- `mutateElites` - When true, chromosomes chosen as elites are mutated (default: false)  
- `eliteMutationRate` - The probability of each city in the DNA of an elite chromosome to be mutated (default: 0.01)  
- `itersToFindNewCity` - The maximum number of iterations to find a new random city during mutation in case two identical cities are chosen (default: 5)  
- `crossoverType` - The selected method of crossing over two chromosomes (default: ERX), available options: Edge Recombination Crossover (ERX) and OX (Ordered Crossover)

> Note: When mutateElites is set to false, the best route in the current generation and the all-time best route will look the same. This is because the amount of randomness in the population introduced by mutation is low.

## Unity Assets
- [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)

## License
[MIT](https://github.com/jonasstr/Genetic-Algorithm-TSP-Unity/blob/master/LICENSE)
