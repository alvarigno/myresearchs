﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccesoDatos.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class bdToolsEntities : DbContext
    {
        public bdToolsEntities()
            : base("name=bdToolsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<Nullable<long>> bdj_idJato_SEL_marca_modelo_version_carroceria_ptas_ano_trans_ltl(string marca, string modelo, string version, string carroceria, Nullable<decimal> ptas, Nullable<decimal> ano, string trans, string ltl)
        {
            var marcaParameter = marca != null ?
                new ObjectParameter("marca", marca) :
                new ObjectParameter("marca", typeof(string));
    
            var modeloParameter = modelo != null ?
                new ObjectParameter("modelo", modelo) :
                new ObjectParameter("modelo", typeof(string));
    
            var versionParameter = version != null ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(string));
    
            var carroceriaParameter = carroceria != null ?
                new ObjectParameter("carroceria", carroceria) :
                new ObjectParameter("carroceria", typeof(string));
    
            var ptasParameter = ptas.HasValue ?
                new ObjectParameter("ptas", ptas) :
                new ObjectParameter("ptas", typeof(decimal));
    
            var anoParameter = ano.HasValue ?
                new ObjectParameter("ano", ano) :
                new ObjectParameter("ano", typeof(decimal));
    
            var transParameter = trans != null ?
                new ObjectParameter("trans", trans) :
                new ObjectParameter("trans", typeof(string));
    
            var ltlParameter = ltl != null ?
                new ObjectParameter("ltl", ltl) :
                new ObjectParameter("ltl", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<long>>("bdj_idJato_SEL_marca_modelo_version_carroceria_ptas_ano_trans_ltl", marcaParameter, modeloParameter, versionParameter, carroceriaParameter, ptasParameter, anoParameter, transParameter, ltlParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_bdj_getNonJatoID(Nullable<decimal> idCategoria, string marca, string modelo, Nullable<decimal> ano, string carroceria, string trans, string combustible)
        {
            var idCategoriaParameter = idCategoria.HasValue ?
                new ObjectParameter("idCategoria", idCategoria) :
                new ObjectParameter("idCategoria", typeof(decimal));
    
            var marcaParameter = marca != null ?
                new ObjectParameter("marca", marca) :
                new ObjectParameter("marca", typeof(string));
    
            var modeloParameter = modelo != null ?
                new ObjectParameter("modelo", modelo) :
                new ObjectParameter("modelo", typeof(string));
    
            var anoParameter = ano.HasValue ?
                new ObjectParameter("ano", ano) :
                new ObjectParameter("ano", typeof(decimal));
    
            var carroceriaParameter = carroceria != null ?
                new ObjectParameter("carroceria", carroceria) :
                new ObjectParameter("carroceria", typeof(string));
    
            var transParameter = trans != null ?
                new ObjectParameter("trans", trans) :
                new ObjectParameter("trans", typeof(string));
    
            var combustibleParameter = combustible != null ?
                new ObjectParameter("combustible", combustible) :
                new ObjectParameter("combustible", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_bdj_getNonJatoID", idCategoriaParameter, marcaParameter, modeloParameter, anoParameter, carroceriaParameter, transParameter, combustibleParameter);
        }
    
        public virtual ObjectResult<string> bdj_Ltl_SEL_marca_modelo_version_carroceria_ptas_ano_trans(string marca, string modelo, string version, string carroceria, Nullable<decimal> ptas, Nullable<decimal> ano, string trans)
        {
            var marcaParameter = marca != null ?
                new ObjectParameter("marca", marca) :
                new ObjectParameter("marca", typeof(string));
    
            var modeloParameter = modelo != null ?
                new ObjectParameter("modelo", modelo) :
                new ObjectParameter("modelo", typeof(string));
    
            var versionParameter = version != null ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(string));
    
            var carroceriaParameter = carroceria != null ?
                new ObjectParameter("carroceria", carroceria) :
                new ObjectParameter("carroceria", typeof(string));
    
            var ptasParameter = ptas.HasValue ?
                new ObjectParameter("ptas", ptas) :
                new ObjectParameter("ptas", typeof(decimal));
    
            var anoParameter = ano.HasValue ?
                new ObjectParameter("ano", ano) :
                new ObjectParameter("ano", typeof(decimal));
    
            var transParameter = trans != null ?
                new ObjectParameter("trans", trans) :
                new ObjectParameter("trans", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("bdj_Ltl_SEL_marca_modelo_version_carroceria_ptas_ano_trans", marcaParameter, modeloParameter, versionParameter, carroceriaParameter, ptasParameter, anoParameter, transParameter);
        }
    }
}
