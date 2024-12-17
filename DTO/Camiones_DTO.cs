using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Camiones_DTO
    {
        //DTO=> DATA TRANSFER OBJECT
        //decoradores de codigo}
        //sirve para dar caracteristicas y definiciones especificas a dada campo y /o elemento de una clase
        [Key]//Datan annotation
        public int ID_Camion { get; set; }
        [Required]
        [Display(Name ="Matrícula")]//dataHelper
        public string Matricula { get; set; }
        [Required]
        [Display(Name = "Tipo_camion")]//dataHelper
        public string Tipo_Camion { get; set; }
        [Required]
        [Display(Name = "Marca")]//dataHelper
        public string Marca { get; set; }
        [Required]
        [Display(Name = "Modelo")]//dataHelper
        public string Modelo { get; set; }
        [Required]
        [Display(Name = "Capacidad")]//dataHelper
        public int Capacidad { get; set; }
        [Required]
        [Display(Name = "Kilometraje")]//dataHelper
        public double Kilometraje { get; set; }

        [DataType(DataType.ImageUrl)]
        public string UrlFoto { get; set; }
        [Required]
        [Display(Name = "Disponibilidad")]//dataHelper
        public bool Disponibilidad { get; set; }
    }
}
