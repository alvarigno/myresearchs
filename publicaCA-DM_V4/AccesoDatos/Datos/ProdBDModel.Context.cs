﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccesoDatos.Datos
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class baseprod2Entities : DbContext
    {
        public baseprod2Entities()
            : base("name=baseprod2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int SP_Datos_Usuario_CADM(string nomAuto, ObjectParameter usr, ObjectParameter pass)
        {
            var nomAutoParameter = nomAuto != null ?
                new ObjectParameter("NomAuto", nomAuto) :
                new ObjectParameter("NomAuto", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Datos_Usuario_CADM", nomAutoParameter, usr, pass);
        }
    }
}
