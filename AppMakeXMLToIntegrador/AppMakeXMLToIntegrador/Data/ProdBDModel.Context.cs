﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppMakeXMLToIntegrador.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
    
        public virtual DbSet<tabautos> tabautos { get; set; }
        public virtual DbSet<tbl_fotosNuevoServer> tbl_fotosNuevoServer { get; set; }
        public virtual DbSet<tabCategorias> tabCategorias { get; set; }
        public virtual DbSet<tabmarcas> tabmarcas { get; set; }
        public virtual DbSet<tabclientes> tabclientes { get; set; }
        public virtual DbSet<tabCategoria_Tipo> tabCategoria_Tipo { get; set; }
        public virtual DbSet<tipos> tipos { get; set; }
        public virtual DbSet<tabcarroceria> tabcarroceria { get; set; }
        public virtual DbSet<tbl_combustible> tbl_combustible { get; set; }
    }
}
