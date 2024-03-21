namespace witchmateCSharp.Models.Functions;

public class FunctionSolution : IFunctionSolution
{
    private readonly List<KeyValuePair<float, float>> _solutions;

    public IReadOnlyCollection<KeyValuePair<float, float>> Solves => _solutions;

    public FunctionSolution()
    {
        _solutions = [];
    }
    
    public FunctionSolution(List<KeyValuePair<float, float>> solutions)
    {
        _solutions = new List<KeyValuePair<float, float>>(solutions);
    }
    
    public bool AddSolution(float x, float f)
    {
        var kv = new KeyValuePair<float, float>(x, f);

        if (_solutions.Contains(kv)) return false;
        _solutions.Add(kv);
        return true;
    }

    public bool RemoveSolution(int index)
    {
        if (_solutions.Count <= index || index < 0) return false;
        _solutions.RemoveAt(index);
        return true;
    }
}