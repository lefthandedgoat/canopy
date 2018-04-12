using System.Collections.Generic;
using _ = canopy.csharp.canopy;
using __ = canopy.csharp.integration;

namespace csharptests
{
    public class jsonValidatorTests
    {
        static string person1 = @"{ ""first"":""jane"", ""middle"":""something"", ""last"":""doe"" }";
        static string person2 = @"{ ""first"":""jane"", ""last"":""doe"" } ";
        static string person3 = @"{ ""first"":""jane"", ""middle"":""something"", ""last"":""doe"", ""phone"":""800-555-5555"" } ";
        
        static string location1 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ 1, 2, 3 ] } ";
        static string location2 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ ] } ";
         
        static string location3 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ { ""first"":""jane"", ""middle"":""something"", ""last"":""doe"" } ] } ";
        static string location4 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ ] } ";
        static string location5 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ { ""first"":""jane"", ""last"":""doe"" } ] } ";
        static string location6 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ { ""first"":""jane"", ""middle"":""something"", ""last"":""doe"", ""phone"":""800-555-5555"" } ] } ";
         
        static string location7 = @"{ ""lat"":4.0212, ""long"":12.102012, ""workers"":[ { ""first"":""jane"", ""last"":""doe"" } ] } ";
        static string location8 = @"{ ""lat"":4.0212, ""long"":12.102012, ""people"":[ { ""first"":""jane"", ""middle"":""something"", ""last"":""doe"", ""phone"":""800-555-5555"" } ] } ";
         
        static string class1 = @"{ ""name"":""bio 101"",  ""building"":""science"", ""location"": { ""lat"":4.0212, ""long"":12.102012, ""people"": [ { ""first"":""jane"", ""middle"":""something"", ""last"":""doe"" } ] } }";
        static string class2 = @"{ ""name"":""chem 101"", ""building"":""science"", ""location"": { ""lat"":4.0212, ""lng"":12.102012,  ""people"": [ { ""first"":""jane"", ""last"":""doe"" } ] } }";

        static List<string> empty = new List<string>();

        public static void All()
        {
            _.context("json validator tests");

            _.test("two identical people have no differences", () =>
            {
                var diff = __.diffJson(person1, person1);
                _.equality(diff.Count, empty.Count);
                __.validateJson(person1, person1);
            });

            _.test("missing property is identified", () =>
            {
                var diff = __.diffJson(person1, person2);
                _.equality(1, diff.Count);
                _.equality(true, diff.Contains("Missing {root}.middle"));
            });

            _.test("extra property is identified", () =>
            {
                var diff = __.diffJson(person1, person3);
                _.equality(1, diff.Count);
                _.equality(true, diff.Contains("Extra {root}.phone"));
            });

            _.test("empty array is acceptable array of ints", () =>
            {
                var diff = __.diffJson(location1, location2);
                _.equality(diff.Count, empty.Count);
                __.validateJson(location1, location2);
            });

            _.test("empty array is acceptable array of records", () =>
            {
                var diff = __.diffJson(location3, location4);
                _.equality(diff.Count, empty.Count);
                __.validateJson(location3, location4);
            });

            _.test("missing fields on records in arrays recognized correctly", () =>
            {
                var diff = __.diffJson(location3, location5);
                _.equality(1, diff.Count);
                _.equality(true, diff.Contains("Missing {root}.[people].{}.middle"));
            });

            _.test("extra fields on records in arrays recognized correctly", () =>
            {
                var diff = __.diffJson(location3, location6);
                _.equality(1, diff.Count);
                _.equality(true, diff.Contains("Extra {root}.[people].{}.phone"));
            });

            _.test("renamed field with extra property shows", () =>
            {
                var diff = __.diffJson(location7, location8);
                _.equality(7, diff.Count);
                _.equality(true, diff.Contains("Missing {root}.[workers]"));

                _.equality(true, diff.Contains("Extra {root}.[people]"));
                _.equality(true, diff.Contains("Extra {root}.[people].{}"));
                _.equality(true, diff.Contains("Extra {root}.[people].{}.first"));
                _.equality(true, diff.Contains("Extra {root}.[people].{}.last"));
                _.equality(true, diff.Contains("Extra {root}.[people].{}.middle"));
                _.equality(true, diff.Contains("Extra {root}.[people].{}.phone"));
            });

            _.test("nested objects with arrays reocgnized correctly", () =>
            {
                var diff = __.diffJson(class1, class2);
                _.equality(3, diff.Count);
                _.equality(true, diff.Contains("Missing {root}.{location}.long"));
                _.equality(true, diff.Contains("Missing {root}.{location}.[people].{}.middle"));

                _.equality(true, diff.Contains("Extra {root}.{location}.lng"));
            });
        }
    }
}
