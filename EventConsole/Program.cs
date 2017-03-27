using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverUrl = "http://eventwsdemo20170327093244.azurewebsites.net";

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri(serverUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //POST: tester om vi kan tilføje en ny event til azure-databasen fra klient-siden:
                Event NewEvent = new Event()
                {
                    //Id = 1,
                    Name = "Søren",
                    Description = "Studerende",
                    Place = "EASJ",
                    Date = new DateTime(2017, 03, 27)
                };
                try
                {
                    var response = client.PostAsJsonAsync<Event>("api/Events", NewEvent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Du har indsat en ny event");
                        Console.WriteLine("Post Content: " + response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        Console.WriteLine("Fejl, din event blev ikke indsat");
                        Console.WriteLine("Statuskode : " + response.StatusCode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Der er sket en fejl : " + e.Message);
                }


                //DELETE
                Console.WriteLine("\nDelete (HTTP Delete) en Event");

                string urlStringDelete = "api/Events/2";

                try
                {
                    HttpResponseMessage response = client.DeleteAsync(urlStringDelete).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Du har slettet et Event");
                        Console.WriteLine("Statuskode : " + response.StatusCode);
                    }
                    else
                    {
                        Console.WriteLine("Fejl, Event blev ikke slettet");
                        Console.WriteLine("Statuskode : " + response.StatusCode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Der er sket en fejl : " + e.Message);
                }




                //GET alle Events

                string urlStringGetAllEvents = "api/Events";

                try
                {
                    HttpResponseMessage responsGetAllEvents = client.GetAsync(urlStringGetAllEvents).Result;
                    if (responsGetAllEvents.IsSuccessStatusCode)
                    {
                        var GetAllEvents = responsGetAllEvents.Content.ReadAsAsync<IEnumerable<Event>>().Result;

                        foreach (var item in GetAllEvents)
                        {
                            Console.WriteLine($"\nGET Events: \n\tEvent id: \t{ item.Id } \n\tName: \t{ item.Name } \n\tDescription: \t{ item.Description } \n\tPlace: {item.Place} \n\tDate: {item.Date}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Der er sket en fejl : " + e.Message);
                }


                //GET specifik Event, fx id = 1

                string urlStringGetSingleEvent = "api/Events/100";

                try
                {
                    HttpResponseMessage responsGetSingleEvents = client.GetAsync(urlStringGetSingleEvent).Result;
                    if (responsGetSingleEvents.IsSuccessStatusCode)
                    {
                        var GetSingleEvents = responsGetSingleEvents.Content.ReadAsAsync<Event>().Result;

                        Console.WriteLine($"\nDu hentede Event-id: {GetSingleEvents.Id} \n\tName: {GetSingleEvents.Name} \n\tDescription: \t{ GetSingleEvents.Description } \n\tPlace: {GetSingleEvents.Place} \n\tDate: {GetSingleEvents.Date}");
                        //foreach (var item in GetAllEvents)
                        //{
                            //Console.WriteLine($"\nGET Events: \n\tEvent id: \t{ item.Id } \n\tName: \t{ item.Name } \n\tDescription: \t{ item.Description } \n\tPlace: {item.Place} \n\tDate: {item.Date}");
                        //}
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Der er sket en fejl : " + e.Message);
                }
            }
        }
    }
}
