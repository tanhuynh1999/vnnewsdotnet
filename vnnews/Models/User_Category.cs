//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vnnews.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Category
    {
        public int uc_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public Nullable<System.DateTime> uc_datecreate { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}