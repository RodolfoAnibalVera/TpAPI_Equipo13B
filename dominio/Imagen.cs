using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Imagen
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("ID Artículo")]
        public int IdArticulo { get; set; }

        [DisplayName("Url de Imagen")]
        public string ImagenUrl { get; set; }

    }
}
