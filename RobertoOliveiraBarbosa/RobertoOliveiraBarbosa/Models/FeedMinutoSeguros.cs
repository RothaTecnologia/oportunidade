using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RobertoOliveiraBarbosa.Models
{
    public class FeedMinutoSeguros
    {
        public String Titulo { get; set; }
        public String Link { get; set; }
        public String Descricao { get; set; }
        public String DataPublicacao { get; set; }
        public int TotalPalavras { get; set; }
        public List<String> Top10Palavras { get; set; }
        public string TopPalavras { get; set; }
    }
}