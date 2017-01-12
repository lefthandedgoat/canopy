using System;
using canopy;
using _ = canopy.csharp.canopy;

namespace csharptests
{
    class Program
    {
        static void Main(string[] args)
        {
            configuration.elementTimeout = 3.0;
            configuration.compareTimeout = 3.0;
            configuration.pageTimeout = 3.0;
            configuration.reporter = new reporters.LiveHtmlReporter(types.BrowserStartMode.Chrome, configuration.chromeDir);

            var testpage = "http://lefthandedgoat.github.io/canopy/testpages/";
            
            _.start(types.BrowserStartMode.Chrome);

            _.context("context1");
            _.once(() => Console.WriteLine("once"));
            _.before(() => Console.WriteLine("before"));
            _.after(() => Console.WriteLine("after"));
            _.lastly(() => Console.WriteLine("lastly"));

            _.skip("intentionally skipped shows blue in LiveHtmlReport", () =>
            {
                _.url("http://www.skipped.com");
            });
            
            _.test("Apostrophes don't break anything", () =>
            {
                _.url(testpage);
                _.count("I've got an apostrophe", 1);
            });

            _.test("#firstName should have John (using == infix operator)", () =>
            {
                _.url(testpage);
                _.eq("#firstName", "John");
            });


            _.test("#lastName should have Doe via read IWebElements", () =>
            {
                _.url(testpage);
                var e = _.element("#lastname");
                var value = _.read(e);
                _.equality(value, "Doe");
            });

            _.test("clearing #firstName sets text to new empty string", () =>
            {
                _.url(testpage);
                _.clear("#firstName");
                _.eq("#firstName", "");
            });

            _.test("clearing #firstName sets text to new empty string via IWebElement", () =>
            {
                _.url(testpage);
                _.clear(_.element("#firstName"));
                _.eq("#firstName", "");
            });

            _.test("writing to #lastName sets text to Smith", () =>
            {
                _.url(testpage);
                _.clear("#lastName");
                _.write("#lastName", "Smith");
                _.eq("#lastName", "Smith");
            });
            
            _.test("writing to #lastName (as element) sets text to John", () =>
            {
                _.url(testpage);
                var lastname = _.element("#lastname");
                _.clear(lastname);
                _.write(lastname, "John");
                _.eq("#lastname", "John");
            });
           
            _.run();

            System.Console.ReadKey();
            _.quit();
        }
    }
}
