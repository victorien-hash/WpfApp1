using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.SharedKernel;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Entites
{
    public class Film: BaseEntity, IAggregateRoot
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CheminImage { get; set; } = string.Empty; // Pour l'affiche
        //public string CheminVideo {  get; set; } = string.Empty;
        public decimal PrixAchat { get; set; } // Coût pour l'admin

        public Film() { }
    }
}
