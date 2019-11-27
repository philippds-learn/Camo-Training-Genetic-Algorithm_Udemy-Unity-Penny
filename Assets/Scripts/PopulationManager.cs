using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject personPrefab;
    public int populationSize = 10;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;

    int trialTime = 10;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + this.generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < this.populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(this.personPrefab, pos, Quaternion.identity);
            DNA dna = go.GetComponent<DNA>();
            dna.r = Random.Range(0.0f, 1.0f);
            dna.g = Random.Range(0.0f, 1.0f);
            dna.b = Random.Range(0.0f, 1.0f);
            dna.scaleFactor = Random.Range(1.0f, 2.0f);
            this.population.Add(go);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(this.personPrefab, pos, Quaternion.identity);
        DNA dna1 = parent1.GetComponent <DNA>();
        DNA dna2 = parent1.GetComponent <DNA>();
        // swap parent dna
        DNA dnaOffspring = offspring.GetComponent<DNA>();
        // mutate chance 50%
        if (Random.Range(0, 2) == 0)
        {            
            dnaOffspring.r = Random.Range(0, 2) == 0 ? dna1.r : dna2.r;
            dnaOffspring.g = Random.Range(0, 2) == 0 ? dna1.g : dna2.g;
            dnaOffspring.b = Random.Range(0, 2) == 0 ? dna1.b : dna2.b;
            dnaOffspring.scaleFactor = Random.Range(0, 2) == 0 ? dna1.scaleFactor : dna2.scaleFactor;
        }
        else
        {
            // every second breed is random colored: mutation
            dnaOffspring.r = Random.Range(0.0f, 1.0f);
            dnaOffspring.g = Random.Range(0.0f, 1.0f);
            dnaOffspring.b = Random.Range(0.0f, 1.0f);
            dnaOffspring.scaleFactor = Random.Range(1.0f, 2.0f);
        }
        
        return offspring;
    }

    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        // get rid of unfit individuals

        // select last what you want to breed
        // List<GameObject> sortedList = this.population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();

        // select first what you want to breed
        List<GameObject> sortedList = this.population.OrderByDescending(o => o.GetComponent<DNA>().timeToDie).ToList();

        population.Clear();
        // breed upper half of sorted List
        for(int i = (int) (sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            this.population.Add(Breed(sortedList[i], sortedList[i + 1]));
            this.population.Add(Breed(sortedList[i+1], sortedList[i]));
        }

        // destroy all parents and previous population
        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);            
        }
        this.generation++;
    }

    // Update is called once per frame
    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > this.trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
