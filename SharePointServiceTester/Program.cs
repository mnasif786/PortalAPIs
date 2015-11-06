using System;
using System.Linq;
using SharePointServiceTester.ServiceReference2;


namespace SharePointServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {

            var svc = new SpServiceClient();
            var locations = svc.GetStationaryLocations();
            var sd = svc.GetStationaryDocuments(locations[0])[0];
            Console.WriteLine(sd.Location);
            Console.WriteLine(sd.CreatedOn);
            Console.WriteLine(sd.Id);
            Console.WriteLine(sd.Title);


            var scwc = svc.GetStationaryDocument(sd.Id);
            Console.WriteLine(scwc.Document.Id);
            Console.WriteLine(scwc.FBytes.Count());

            Console.Read();

        }
    }
}
