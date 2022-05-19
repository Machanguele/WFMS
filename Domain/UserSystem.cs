using System;

namespace Domain
{
    public class UserSystem
    {
        public int Id { get; set; }
        public virtual AppUser AppUser{ get; set; }
        public DateTime LastUpdate{ get; set; }
        public bool LastUpdateCommittedToTargetSystem{ get; set; }
        public virtual EcoSystem EcoSystem{ get; set; }
    }
}