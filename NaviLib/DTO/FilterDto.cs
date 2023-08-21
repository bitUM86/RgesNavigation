
namespace NaviLib.DTO
{
    public class FilterDto
    {
        public string Name { get; set; }
        public List<string> Filter { get; set; }
        public List<string> District { get; set; }

        public FilterDto(string name, List<string> filter, List<string> district )
        {
            Name = name;
            Filter = filter;
            District = district;
        }


    }
}
