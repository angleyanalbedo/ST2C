using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st2c.JSON
{

    public static class Format
    {
        public static string SetString(string[] stringVar)
        {
            foreach (string elemVar in stringVar)
            {
                // 处理字符串逻辑
            }
            return null;
        }

        public static void JsonToString(string jsonString)
        {
            string pretty = ToPrettyFormat(jsonString);
            Console.WriteLine(pretty);
        }

        /// <summary>
        /// 格式化输出JSON字符串
        /// </summary>
        /// <returns>格式化后的JSON字符串</returns>
        private static string ToPrettyFormat(string json)
        {
            var jsonObj = JToken.Parse(json);
            return jsonObj.ToString(Formatting.Indented);
        }

        public static void TestOfJson(int jsonNumber)
        {
            var jsonObject = new JObject();
            jsonObject["int"] = jsonNumber;
            Console.WriteLine("json" + jsonObject);

            string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            Console.WriteLine("formatted" + formattedJson);
        }

        public static string ToPrettyString(JToken jsonElement)
        {
            return jsonElement.ToString(Formatting.Indented);
        }

        public static void PrintlnJsonObjectString(JToken jsonObject)
        {
            Console.WriteLine(ToPrettyString(jsonObject));
        }
    }
}
