using Newtonsoft.Json;
using System;

namespace StackUnderflow.Bots
{
    public static class Logging
    {
        public static void ConsoleLogAsIdentedJson(this object arg)
        {
            Console.WriteLine(JsonConvert.SerializeObject(arg, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
    }
}
