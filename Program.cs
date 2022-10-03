using Saxon.Api;
using System.Xml;

var processor = new Processor();

var docBuilder = processor.NewDocumentBuilder();

docBuilder.LineNumbering = true;

docBuilder.BaseUri = new Uri("urn:from-string");

const string mixedContent = @"<text>This is <b id=""b1"" style=""font-weight: bold;"">mixed</b> content: <!-- test -->Can XPath tools select a text node?</text>";

var xdmDoc = docBuilder.Build(new StringReader(mixedContent));

foreach (XdmNode node in processor.NewXPathCompiler().Evaluate("//*[@*]/(., @*)", xdmDoc))
{
    Console.WriteLine($"Node {node}; line: {node.LineNumber}; column: {node.ColumnNumber}");
}

using (var xr = XmlReader.Create(new StringReader(mixedContent)))
{
    while (xr.Read())
    {
        if (xr.HasAttributes)
        {
            Console.WriteLine($"Node {xr.NodeType}; Value: {xr.Value}; Line: {((IXmlLineInfo)xr).LineNumber}; Column: {((IXmlLineInfo)xr).LinePosition}");
            xr.MoveToFirstAttribute();
            do
            {
                Console.WriteLine($"Node {xr.NodeType}; Value: {xr.Value}; Line: {((IXmlLineInfo)xr).LineNumber}; Column: {((IXmlLineInfo)xr).LinePosition}");
            }
            while (xr.MoveToNextAttribute());
        }
    }
}

using (var xr = XmlReader.Create(new StringReader(mixedContent)))
{
    xdmDoc = docBuilder.Build(xr);

    foreach (XdmNode node in processor.NewXPathCompiler().Evaluate("//@*", xdmDoc))
    {
        Console.WriteLine($"Node {node}; line: {node.LineNumber}; column: {node.ColumnNumber}");
    }
}


