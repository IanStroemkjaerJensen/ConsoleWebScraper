

using AngleSharp;
using AngleSharp.Dom;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    static readonly HttpClient _httpClient = new HttpClient();
    static async Task Main(string[] args) {

        HttpResponseMessage _responsMessage = await _httpClient.GetAsync("https://www.timeanddate.no/vaer/?continent=europe&low=c");
        _responsMessage.EnsureSuccessStatusCode();
        string _responseBody = await _responsMessage.Content.ReadAsStringAsync();

        var _context = BrowsingContext.New(Configuration.Default);
        var _document = await _context.OpenAsync(req => req.Content(_responseBody));

        var _nodesList = _document.QuerySelectorAll("table tr");
        var _nodes = _nodesList.ToList().GetRange(1, (_nodesList.ToList().Count()-1));

        var _childNodes = _nodes.Select(node => node.ChildNodes.ToArray());

        StringBuilder _stringBuilder = new();

        foreach(var _node in _childNodes)
        {
            foreach(var _var in _node)
            {
                _stringBuilder.Append(_var.Text());
            }
        }
        string weather = _stringBuilder.ToString();
        weather = Regex.Replace(weather, "[æ ø å a-z] {3} \\s [0-9] {2} : [0-9] {2}", "");
        weather = Regex.Replace(weather, "\\s °C", "°C;");
        weather = Regex.Replace(weather, "[*]\\s", "");

        string[] stringArrayForPrinting = weather.Split(';');

        Array.Sort(stringArrayForPrinting);

        for(int i = 0; i < stringArrayForPrinting.Length; i++)
       
        { Console.WriteLine(stringArrayForPrinting[i]); }
        Console.ReadLine();
    }
 }