using HtmlAgilityPack;
using RobertoOliveiraBarbosa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace RobertoOliveiraBarbosa.Controllers
{
    public class FeedMinutoSegurosController : Controller
    {
        // GET: FeedMinutoSeguros
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpPost]
        public ActionResult Index()
        {
            String url = "https://www.minutoseguros.com.br/blog/feed/";
            //WebClient client = new WebClient();
            //string rss = client.DownloadString(url);
            //XDocument xml = XDocument.Parse(rss);
            //Response.ContentType = "text/xml";
            XDocument xml = XDocument.Load(Server.MapPath("/XML/feed.xml"));
            
            var rssData = (from x in xml.Descendants("item")
                           select new FeedMinutoSeguros
                           {
                               Titulo = ((string)x.Element("title")),
                               Link = ((string)x.Element("link")),
                               Descricao = ((string)x.Element("description")),
                               DataPublicacao = ((string)x.Element("pubDate"))
                           });


            List<FeedMinutoSeguros> lst = new List<FeedMinutoSeguros>();

            foreach (var itemTratado in rssData)
            {
                FeedMinutoSeguros feed = new FeedMinutoSeguros();
                feed.DataPublicacao = itemTratado.DataPublicacao;
                feed.Descricao = Server.HtmlDecode(itemTratado.Descricao);
                feed.Link = itemTratado.Link;
                feed.Titulo = itemTratado.Titulo;
                char[] separador = new char[] { ' ', '\t' };
                
                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(itemTratado.Descricao);
                
                feed.TotalPalavras = Server.HtmlDecode(html.DocumentNode.InnerText).Split(separador).OfType<String>()
                                                                        .ToList()
                                                                        .GroupBy(s => s)
                                                                        .Select(g => new { Chave = g.Key, Valor = g.Count() })
                                                                        .Where(n => n.Chave.ToLower() != "de" && n.Chave.ToLower() != "o" && n.Chave.ToLower() != "um"
                                                                        && n.Chave.ToLower() != "para" && n.Chave.ToLower() != "uma" && n.Chave.ToLower() != "é"
                                                                        && string.IsNullOrEmpty(n.Chave) == false && n.Chave.ToLower() != "a"
                                                                        && n.Chave.ToLower() != "sua" && n.Chave.ToLower() != "que"
                                                                        && n.Chave.ToLower() != "em" && n.Chave.ToLower() != "do"
                                                                        && n.Chave.ToLower() != "as" && n.Chave.ToLower() != "não"
                                                                        && n.Chave.ToLower() != "com" && n.Chave.ToLower() != "no"
                                                                        && n.Chave.ToLower() != "nos" && n.Chave.ToLower() != "e"
                                                                        && n.Chave.ToLower() != "os").Count();

                var listaTop = Server.HtmlDecode(html.DocumentNode.InnerText).Split(separador).OfType<String>()
                                                                        .ToList()
                                                                        .GroupBy(s => s)
                                                                        .Select(g => new { Chave = g.Key, Valor = g.Count()})
                                                                        .Where(n => n.Chave.ToLower() != "de" && n.Chave.ToLower() != "o" && n.Chave.ToLower() != "um"
                                                                        && n.Chave.ToLower() != "para" && n.Chave.ToLower() != "uma" && n.Chave.ToLower() != "é"
                                                                        && string.IsNullOrEmpty(n.Chave) == false && n.Chave.ToLower() != "a"
                                                                        && n.Chave.ToLower() != "sua" && n.Chave.ToLower() != "que" && n.Chave.ToLower() != "em"
                                                                        && n.Chave.ToLower() != "do" && n.Chave.ToLower() != "as" && n.Chave.ToLower() != "não"
                                                                        && n.Chave.ToLower() != "com" && n.Chave.ToLower() != "no" && n.Chave.ToLower() != "nos"
                                                                        && n.Chave.ToLower() != "e" && n.Chave.ToLower() != "os")
                                                                        .Take(10)
                                                                        .OrderByDescending(x => x.Valor);
                foreach (var palavras in listaTop)
                {
                    if (string.IsNullOrEmpty(feed.TopPalavras))
                    {
                        feed.TopPalavras = String.Format("{0} - ({1}x)<br>", palavras.Chave.ToString(), palavras.Valor.ToString());
                    }
                    else
                    {
                        feed.TopPalavras += string.Format(",{0}  - ({1}x)<br>", palavras.Chave.ToString(), palavras.Valor.ToString());
                    }
                }
                
                lst.Add(feed);
            }

            ViewBag.Feed = lst;
            ViewBag.URL = url;

            return View();
        }
    }
}