namespace witchmateCSharp.Models.Functions;

public interface IFunctionSolution
{
    public IReadOnlyCollection<KeyValuePair<float, float>> Solves { get; }
    
    public bool AddSolution(float x, float f);
    public bool RemoveSolution(int index);
}