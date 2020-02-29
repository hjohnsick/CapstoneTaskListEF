using System;
using System.Collections.Generic;

namespace EFCoreIdentityFramework.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Complete { get; set; }
        public string OwnerId { get; set; }

        public virtual AspNetUsers Owner { get; set; }
    }
}
