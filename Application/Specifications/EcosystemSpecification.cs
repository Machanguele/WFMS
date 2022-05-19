using Domain;

namespace Application.Specifications
{
    public class EcosystemSpecification : SpecificationBase<EcoSystem>
    {
        public EcosystemSpecification(string url, string description)
        : base(x=>x.Description.ToLower() == description.ToLower() || x.Url.ToLower() == url.ToLower())
        {
            
        }
        
        public EcosystemSpecification(string url, string description, int id)
        : base(x=>(x.Description.ToLower() == description.ToLower() || x.Url.ToLower() == url.ToLower())
                  && x.Id != id)
        {
            
        }
    }
}