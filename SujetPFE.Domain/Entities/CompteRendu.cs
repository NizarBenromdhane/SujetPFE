﻿using System;

namespace SujetPFE.Domain.Entities
{
    public class CompteRendu
    {
        public int Id { get; set; }
        public int RDVId { get; set; }  // Clé étrangère vers RDV
        public DateTime DateCreation { get; set; }
        public string Contenu { get; set; }

        // Navigation property
        public RDV RDV { get; set; }
    }
}