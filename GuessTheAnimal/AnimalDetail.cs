//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuessTheAnimal
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnimalDetail
    {
        public int FactId { get; set; }
        public string Facts { get; set; }
        public int AnimalId { get; set; }
    
        public virtual Animal Animal { get; set; }
    }
}