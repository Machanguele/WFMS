using Domain;

namespace Application.Specifications
{
    public class ProjectSpecification : SpecificationBase<Project>
    {
        //: base(x=>x.Description.ToLower() == description.ToLower() || x.Url.ToLower() == url.ToLower())
        public ProjectSpecification(string projectName): base(x=>x.Name.ToLower() == projectName.ToLower())
        {
            
        }
        
    }
}