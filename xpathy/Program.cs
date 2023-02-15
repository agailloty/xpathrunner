using HtmlAgilityPack;

var doc = new HtmlDocument();
doc.Load("jobOffers.html");

var jobTitles = doc.DocumentNode.SelectNodes("//span[starts-with(@id, 'jobTitle')]/text()");
foreach (var jobTitle in jobTitles)
    Console.WriteLine(jobTitle.InnerText); 