//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trasportes_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Camiones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Camiones()
        {
            this.Rutas = new HashSet<Rutas>();
        }
    
        public int ID_Camion { get; set; }
        public string Matricula { get; set; }
        public string Tipo_Camion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Capacidad { get; set; }
        public double Kilometraje { get; set; }
        public string UrlFoto { get; set; }
        public bool Disponibilidad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rutas> Rutas { get; set; }
    }
}
