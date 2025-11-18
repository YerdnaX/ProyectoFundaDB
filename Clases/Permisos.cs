using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Permisos
    {
        public static bool PuedeEliminar(string rol)
        {
            if (string.IsNullOrEmpty(rol)) return false;

            return rol.ToUpper() switch
            {
                "ADMIN" => true,
                "EDITOR" => false,
                "LECTOR" => false,
                _ => false
            };
        }

        public static bool PuedeEditar(string rol)
        {
            if (string.IsNullOrEmpty(rol)) return false;

            return rol.ToUpper() switch
            {
                "ADMIN" => true,
                "EDITOR" => true,
                "LECTOR" => false,
                _ => false
            };
        }

        public static bool PuedeAgregar(string rol)
        {
            if (string.IsNullOrEmpty(rol)) return false;

            return rol.ToUpper() switch
            {
                "ADMIN" => true,
                "EDITOR" => true,
                "LECTOR" => false,
                _ => false
            };
        }

        public static bool EsAdministrador(string rol)
        {
            return rol?.ToUpper() == "ADMIN";
        }
    }
}

