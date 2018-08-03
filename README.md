# Genetic Algorithm for TSP in Unity

This Unity project can be used to visualize solving the Travelling Salesman Problem (TSP) using an implementation of the Genetic Algorithm.

![alt text](/images/tsp.png?raw=true)

## Installation
Clone or download the project and open it in Unity (File --> Open Project).

## Parameters
The following variables can be adjusted in the Unity inspector by clicking on the World object in the hierarchy.

`cityCount` - The number of cities in a route (default: 20)  
`populationSize` - The number of individuals in the population (default: 18)  
`mutationRate` - The probability of each city in the DNA of a chromosome to be mutated (default: 0.013)  
`elitism` - The number of best chromosomes which are transferred to the next generation (default: 6)  
`mutateElites` - When true, chromosomes chosen as elites are mutated (default: false)  
`eliteMutationRate` - The probability of each city in the DNA of an elite chromosome to be mutated (default: 0.01)  
`itersToFindNewCity` - The maximum number of iterations to find a new random city during mutation in case two identical cities are chosen (default: 5)  
`CrossoverType` - The selected method of crossing over two chromosomes (default: ERX)  

## Unity Assets
- [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)
