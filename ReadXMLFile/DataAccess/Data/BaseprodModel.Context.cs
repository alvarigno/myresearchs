﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class baseprodEntities : DbContext
    {
        public baseprodEntities()
            : base("name=baseprodEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual int SPR_Valida_xKey_Acceso_Usuario(string xkey, ObjectParameter respuesta, ObjectParameter nombre)
        {
            var xkeyParameter = xkey != null ?
                new ObjectParameter("xkey", xkey) :
                new ObjectParameter("xkey", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SPR_Valida_xKey_Acceso_Usuario", xkeyParameter, respuesta, nombre);
        }
    }
}
