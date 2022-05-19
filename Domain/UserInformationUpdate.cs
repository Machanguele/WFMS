using System;

namespace Domain
{
    public class UserInformationUpdate
    {
        public int Id { get; set; }
        public virtual AppUser AppUser { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool LastUpdateCommited { get; set; }
        public virtual UserCrudAction UserCrudAction { get; set; }
    }
}