using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class AsignacionBoton
    {
        public int IdMiembro;
        public int NumeroBoton;
        public string RutaImagen;
        public string NombreImagen;
        public Miembros Miembro;

        public int Id_Miembro { get => IdMiembro; set => IdMiembro = value; }
        public int Numero_Boton { get => NumeroBoton; set => NumeroBoton = value; }
        public string Ruta_Imagen { get => RutaImagen; set => RutaImagen = value; }
        public string Nombre_Imagen { get => NombreImagen; set => NombreImagen = value; }
        public Miembros Miembro_Asociado { get => Miembro; set => Miembro = value; }
    }
}
