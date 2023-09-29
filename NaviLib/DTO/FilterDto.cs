namespace NaviLib.DTO;

public class FilterDto
{
    public string Name { get; }
    public List<string> Filter { get; }
    public List<string> District { get; }

    public FilterDto(string name, List<string> filter, List<string> district )
    {
        Name = name;
        Filter = filter;
        District = district;
    }


}