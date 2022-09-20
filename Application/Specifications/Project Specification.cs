using Domain;

namespace Application.Specifications
{
    public class ComponentSpecification : SpecificationBase<Component>
    {
        //: base(x=>x.Description.ToLower() == description.ToLower() || x.Url.ToLower() == url.ToLower())
        public ComponentSpecification(string projectName): base(x=>x.Title.ToLower() == projectName.ToLower())
        {
            
        }
        
    }
}