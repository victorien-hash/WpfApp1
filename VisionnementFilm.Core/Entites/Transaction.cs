using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionnementFilm.SharedKernel;
using VisionnementFilm.SharedKernel.Interface;

namespace VisionnementFilm.Core.Entites
{
    public class Transaction : BaseEntity, IAggregateRoot
    {
        public int UtilisateurId { get; set; }
        public DateTime DateTransaction { get; set; } = DateTime.Now;
        public string Type { get; set; } = string.Empty; // "Dépôt", "Abonnement", "Remboursement"
        public decimal Montant { get; set; }
    }
}
