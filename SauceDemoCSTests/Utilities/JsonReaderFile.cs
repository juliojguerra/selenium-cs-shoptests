using System;
using Newtonsoft.Json.Linq;

namespace SauceDemoCSTests.Utilities
{
	public class JsonReaderFile
    {
        public string ExtractData(String tokenName)
        {
            String myJsonString = File.ReadAllText("utilities/testData.json");

            var jsonObject = JToken.Parse(myJsonString);

            return jsonObject.SelectToken(tokenName).Value<string>();
        }

    }
}

