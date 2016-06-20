using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.logic;

namespace SpiderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            PicSpider logic = new PicSpider();
            string result = logic.SpiderDo("http://huaban.com/search/?q=car&intxsxgw&page=2&per_page=20&wfl=1", "", "");
            Regex regex = new Regex("csrf-token\" content=\"(?<token>[\\s\\S]*?)\" />", RegexOptions.Compiled);
            if (regex.IsMatch(result))
            {
                result = regex.Match(result).Groups["token"].Value;
            }
            result = logic.SpiderDo("https://api.500px.com/v1/photos?rpp=50&feature=popular&image_size%5B%5D=1&image_size%5B%5D=2&image_size%5B%5D=32&image_size%5B%5D=31&image_size%5B%5D=33&image_size%5B%5D=34&image_size%5B%5D=35&image_size%5B%5D=36&image_size%5B%5D=2048&image_size%5B%5D=4&sort=&include_states=true&formats=jpeg%2Clytro&only=&page=1&rpp=50", "https://500px.com/popular", result);
            logic.ConvertJson(result);

            logic.DelImage();

            Console.ReadLine();
        }
    }
}
