using System;
using CsvHelper;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyExchange
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GetHtML();

        }

        static async Task GetHtML()
        {
            string url = "https://bnb.bg/Statistics/StExternalSector/StExchangeRates/StERForeignCurrencies/index.htm";

            HttpClient client = new HttpClient();

            try
            {
                string html = await client.GetStringAsync(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                HtmlNodeCollection tableNodes = doc.DocumentNode.SelectNodes("//div[@class='table_box']");

                List<Currency> currentCurrency = new List<Currency>();

                if (tableNodes != null)
                {
                    foreach (var node in tableNodes)
                    {
                        var tbodyNodes = node.SelectNodes(".//tbody");

                        if (tbodyNodes != null)
                        {
                            foreach (var tbody in tbodyNodes)
                            {
                                var firstTrNodes = tbody.SelectNodes(".//tr[@class='first']");
                                var restTrNodes = tbody.SelectNodes(".//tr");

                                if (firstTrNodes != null)
                                {
                                    foreach (var tr in firstTrNodes)
                                    {
                                        HtmlNode firstTd = tr.SelectSingleNode(".//td[@class='first']");
                                        HtmlNode centerTd1 = tr.SelectSingleNode(".//td[@class='center']");
                                        HtmlNode rightTd = tr.SelectSingleNode(".//td[@class='right']");
                                        HtmlNode centerTd2 = tr.SelectSingleNode(".//td[@class='center'][2]"); // second center column
                                        HtmlNode lastCenterTd = tr.SelectSingleNode(".//td[@class='last center']");

                                        Currency currency = new Currency(firstTd.InnerText.ToString(), centerTd1.InnerText.ToString(),
                                            rightTd.InnerText.ToString(), centerTd2.InnerText.ToString(), lastCenterTd.InnerText.ToString());
                                        currentCurrency.Add(currency);

                                    }
                                }

                                if (restTrNodes != null)
                                {
                                    foreach (var tr in restTrNodes)
                                    {
                                        HtmlNode firstTd = tr.SelectSingleNode(".//td[@class='first']");
                                        HtmlNode centerTd1 = tr.SelectSingleNode(".//td[@class='center']");
                                        HtmlNode rightTd = tr.SelectSingleNode(".//td[@class='right']");
                                        HtmlNode centerTd2 = tr.SelectSingleNode(".//td[@class='center'][2]"); // second center column
                                        HtmlNode lastCenterTd = tr.SelectSingleNode(".//td[@class='last center']");

                                        if (firstTd == null || centerTd1 == null || rightTd == null
                                            || centerTd2 == null || lastCenterTd == null)
                                            continue;

                                        Currency currency = new Currency(firstTd.InnerText.ToString(), centerTd1.InnerText.ToString(),
                                            rightTd.InnerText.ToString(), centerTd2.InnerText.ToString(), lastCenterTd.InnerText.ToString());
                                        currentCurrency.Add(currency);

                                    }
                                }
                            }
                        }
                    }
                }

                foreach (Currency cur in currentCurrency)
                {
                    Console.WriteLine(cur.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }

        }
    }
}
